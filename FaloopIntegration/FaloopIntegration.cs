using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;
using Divination.FaloopIntegration.Config;
using Divination.FaloopIntegration.Faloop;
using Divination.FaloopIntegration.Faloop.Model;
using Divination.FaloopIntegration.Ipc;
using Divination.FaloopIntegration.Ui;
using Lumina.Excel.Sheets;
using SocketIOClient;

namespace Divination.FaloopIntegration;

public sealed class FaloopIntegration : DivinationPlugin<FaloopIntegration, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private readonly FaloopSocketIOClient socket = new();
    private readonly FaloopSession session = new();
    public readonly ActiveMobUi Ui;
    private World? currentWorld;
    private World? homeWorld;
    public PluginStatus Status;
    private List<MobSpawnEvent> spawnEvents = [];
    private CancellationTokenSource? connectionCancellation;
    private TaskCompletionSource disconnected = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan MinReconnectDelay = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan MaxReconnectDelay = TimeSpan.FromMinutes(5);
    // spawn reports held back because their location is not yet known; announced once spawn_location arrives
    private readonly Dictionary<string, MobSpawnEvent> pendingSpawnEvents = [];

    public FaloopIntegration(IDalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        Config.Migrate();

        socket.OnConnected += OnConnected;
        socket.OnDisconnected += OnDisconnected;
        socket.OnError += OnError;
        socket.OnMobReport += OnMobReport;
        socket.OnAny += OnAny;
        socket.OnPing += OnPing;
        socket.OnPong += OnPong;

        Dalamud.Framework.RunOnFrameworkThread(OnLogin);
        Dalamud.ClientState.Login += OnLogin;

        var ipc = new AetheryteLinkInChatIpc(pluginInterface, Divination.Chat);
        Ui = new ActiveMobUi(ipc, Divination.Chat, Dalamud.GameGui, Config, Dalamud.Condition)
        {
            IsDrawing = Config.EnableActiveMobUi
        };
        Dalamud.PluginInterface.UiBuilder.Draw += Ui.Draw;

        Connect();
    }

    private void OnLogin()
    {
        currentWorld = Dalamud.ObjectTable.LocalPlayer?.CurrentWorld.Value;
        homeWorld = Dalamud.ObjectTable.LocalPlayer?.HomeWorld.Value;
    }

    private void OnConnected()
    {
        Status = PluginStatus.Connected;
        Divination.Chat.Print(Localization.Connected);
    }

    private void OnDisconnected(string cause)
    {
        Status = PluginStatus.Disconnected;
        Divination.Chat.Print(Localization.Disconnected);
        DalamudLog.Log.Warning("Disconnected = {Cause}", cause);

        // this runs inside the Socket.IO client, so only signal the connection loop here
        disconnected.TrySetResult();
    }

    private static void OnError(string error)
    {
        DalamudLog.Log.Error("Error = {Error}", error);
    }

    private void OnMobReport(MobReportData data)
    {
        if (!FaloopEmbedData.Mobs.TryGetValue(data.Ids.MobId, out var mobData))
        {
            DalamudLog.Log.Warning("OnMobReport: mobData == null");
            return;
        }

        if (!FaloopEmbedData.Worlds.TryGetValue(data.Ids.WorldId, out var worldId))
        {
            DalamudLog.Log.Warning("OnMobReport: worldId == null");
            return;
        }

        var config = mobData.Rank switch
        {
            MobRank.S => Config.RankS,
            MobRank.FATE => Config.Fate,
            _ => default,
        };
        if (config == default)
        {
            DalamudLog.Log.Debug("OnMobReport: config == null");
            return;
        }

        if (!CheckSpawnNotificationCondition(config, worldId, mobData.Expansion))
        {
            return;
        }

        switch (data.Action)
        {
            case MobReportActions.Spawn when config.EnableSpawnReport:
                {
                    var spawn = JsonSerializer.Deserialize<MobReportData.Spawn>(data.Data) ?? throw new InvalidOperationException("invalid spawn data");
                    if (!FaloopEmbedData.TerritoryTypes.TryGetValue(spawn.ZoneId, out var territoryTypeId))
                    {
                        DalamudLog.Log.Warning("OnMobReport: unknown zone id found: {ZoneId}", spawn.ZoneId);
                        return;
                    }

                    string? location;
                    if (!string.IsNullOrEmpty(spawn.Location))
                    {
                        location = spawn.Location;
                    }
                    else if (!FaloopEmbedData.Locations.TryGetValue(spawn.ZonePoiIds?.FirstOrDefault() ?? spawn.ZonePoiId ?? default, out location))
                    {
                        DalamudLog.Log.Debug("OnMobReport: unknown location found: {Spawn}", spawn);
                    }

                    var ev = new MobSpawnEvent(mobData.BNpcId, worldId, territoryTypeId, data.Ids.ZoneInstance, mobData.Rank, spawn.Timestamp, spawn.Reporters?.FirstOrDefault()?.Name, location);
                    if (location == default && config.SkipPendingReport)
                    {
                        // keep it instead of dropping, so that a later spawn_location can announce it
                        pendingSpawnEvents[ev.Id] = ev;
                        DalamudLog.Log.Debug("OnMobReport: pending spawn report");
                        return;
                    }

                    OnMobSpawn(ev, config);
                    DalamudLog.Log.Verbose("OnMobReport: OnSpawnMobReport");
                    break;
                }
            case MobReportActions.SpawnLocation when config.EnableSpawnReport:
                {
                    var spawn = JsonSerializer.Deserialize<MobReportData.SpawnLocation>(data.Data) ?? throw new InvalidOperationException("invalid spawn location data");
                    string? location;
                    if (!string.IsNullOrEmpty(spawn.Location))
                    {
                        location = spawn.Location;
                    }
                    else if (!FaloopEmbedData.Locations.TryGetValue(spawn.ZonePoiId ?? default, out location))
                    {
                        DalamudLog.Log.Debug("OnMobReport: unknown location found: {Spawn}", spawn);
                        if (config.SkipPendingReport)
                        {
                            return;
                        }
                    }

                    var id = $"{mobData.BNpcId}_{worldId}_{data.Ids.ZoneInstance}";
                    var previous = spawnEvents.LastOrDefault(x => x.Id == id);
                    if (pendingSpawnEvents.Remove(id, out var pending))
                    {
                        previous ??= pending;
                    }
                    if (previous == default)
                    {
                        DalamudLog.Log.Debug("OnMobReport: previous == null");
                        break;
                    }
                    if (pending == default && previous.Location == location)
                    {
                        DalamudLog.Log.Debug("OnMobReport: location unchanged");
                        break;
                    }
                    var ev = previous with { Location = location };
                    OnMobSpawn(ev, config);
                    break;
                }
            case MobReportActions.SpawnFalse when config.EnableSpawnReport:
                {
                    var ev = new MobDeathEvent(mobData.BNpcId, worldId, data.Ids.ZoneInstance, mobData.Rank, DateTime.Now);
                    OnMobFalseSpawn(ev);
                    break;
                }
            case MobReportActions.Death:
                var death = JsonSerializer.Deserialize<MobReportData.Death>(data.Data) ?? throw new InvalidOperationException("invalid death data");
                OnMobDeath(new MobDeathEvent(mobData.BNpcId, worldId, data.Ids.ZoneInstance, mobData.Rank, death.StartedAt),
                    config.Channel, config.SkipOrphanReport, config.EnableDeathReport);
                DalamudLog.Log.Verbose("OnMobReport: OnDeathMobReport");
                break;
        }
    }

    private bool CheckSpawnNotificationCondition(PluginConfig.PerRankConfig config, uint worldId, GameExpansion expansion)
    {
        if (!Dalamud.DataManager.GetExcelSheet<World>().TryGetRow(worldId, out var world))
        {
            DalamudLog.Log.Debug("OnMobReport: world == null");
            return false;
        }
        var dataCenter = world.DataCenter.ValueNullable;
        if (!dataCenter.HasValue)
        {
            DalamudLog.Log.Debug("dataCenter == null");
            return false;
        }

        if (!config.Jurisdictions.TryGetValue(expansion, out var jurisdiction))
        {
            DalamudLog.Log.Debug("OnMobReport: MajorPatches");
            return false;
        }

        if (config.DisableInDuty && Utils.IsInDuty(Dalamud.Condition))
        {
            DalamudLog.Log.Debug("OnMobReport: in duty");
            return false;
        }

        var currentDataCenter = currentWorld?.DataCenter.ValueNullable;
        var homeDataCenter = homeWorld?.DataCenter.ValueNullable;
        if (!currentWorld.HasValue || !currentDataCenter.HasValue|| !homeDataCenter.HasValue)
        {
            DalamudLog.Log.Debug("OnMobReport: currentWorld == null || currentDataCenter == null || homeDataCenter == null");
            return false;
        }

        switch (jurisdiction)
        {
            case Jurisdiction.All:
            case Jurisdiction.Travelable when dataCenter.Value.Region.RowId == 4 || dataCenter.Value.Region.RowId == homeDataCenter.Value.Region.RowId:
            case Jurisdiction.Region when dataCenter.Value.Region.RowId == currentDataCenter.Value.Region.RowId:
            case Jurisdiction.DataCenter when dataCenter.Value.RowId == currentDataCenter.Value.RowId:
            case Jurisdiction.World when world.RowId == currentWorld.Value.RowId:
                return true;
            default:
                DalamudLog.Log.Verbose("OnMobReport: unmatched");
                return false;
        }
    }

    private void OnMobSpawn(MobSpawnEvent ev, PluginConfig.PerRankConfig config)
    {
        Ui.OnMobSpawn(ev);

        // replace the previous report of the same mob so that location updates do not pile up
        spawnEvents.RemoveAll(x => x.Id == ev.Id);
        spawnEvents.Add(ev);

        var payloads = new List<Payload>();
        if (Config.EnableSimpleReports)
        {
            payloads.Add(new TextPayload($"{SeIconChar.BoxedPlus.ToIconString()}"));
        }
        payloads.Add(new TextPayload(Utils.GetRankIcon(ev.Rank)));
        payloads.Add(new TextPayload($" {ev.Mob.Singular.ExtractText()} "));

        // append MapLink only if pop location is known
        if (ev.Coordinates.HasValue)
        {
            var mapLink = Utils.CreateMapLink(ev.TerritoryType, ev.Map, ev.Coordinates.Value, ev.ZoneInstance);
            payloads.AddRange(mapLink.Payloads);
        }
        else
        {
            payloads.Add(new TextPayload(Utils.GetInstanceIcon(ev.ZoneInstance)));
        }

        payloads.Add(new IconPayload(BitmapFontIcon.CrossWorld));

        if (Config.EnableSimpleReports)
        {
            payloads.Add(new TextPayload($"{ev.World.Name}".TrimEnd()));
        }
        else
        {
            payloads.Add(new TextPayload($"{ev.World.Name} {Localization.HasSpawned} {Utils.FormatTimeSpan(ev.SpawnedAt)}".TrimEnd()));
        }

        Dalamud.ChatGui.Print(new XivChatEntry
        {
            Name = ev.Reporter ?? "Faloop",
            Message = new SeString(payloads),
            Type = Enum.GetValues<XivChatType>()[config.Channel],
        });

        if (SoundEffect.GetSpawnSoundEffectId(config) is { } soundEffectId)
        {
            Dalamud.Framework.RunOnFrameworkThread(() => SoundEffect.Play(soundEffectId));
        }
    }

    private void OnMobFalseSpawn(MobDeathEvent ev)
    {
        Ui.OnMobDeath(ev);

        spawnEvents.RemoveAll(x => x.Id == ev.Id);
        pendingSpawnEvents.Remove(ev.Id);
    }

    private void OnMobDeath(MobDeathEvent ev, int channel, bool skipOrphanReport, bool enableDeathReport)
    {
        Ui.OnMobDeath(ev);
        pendingSpawnEvents.Remove(ev.Id);

        if (skipOrphanReport && spawnEvents.RemoveAll(x => x.Id == ev.Id) == 0)
        {
            DalamudLog.Log.Debug("OnDeathMobReport: skipOrphanReport");
            return;
        }

        if (!enableDeathReport)
        {
            return;
        }

        var payloads = new List<Payload>();
        if (Config.EnableSimpleReports)
        {
            payloads.AddRange(
            [
                new TextPayload($"{SeIconChar.Cross.ToIconString()}"),
                new TextPayload($" {ev.Mob.Singular.ExtractText()}"),
                new TextPayload(Utils.GetInstanceIcon(ev.ZoneInstance)),
                new IconPayload(BitmapFontIcon.CrossWorld),
                new TextPayload($"{ev.World.Name}".TrimEnd()),
            ]);
        }
        else
        {
            payloads.AddRange(
            [
                new TextPayload(Utils.GetRankIcon(ev.Rank)),
                new TextPayload($" {ev.Mob.Singular.ExtractText()}"),
                new TextPayload(Utils.GetInstanceIcon(ev.ZoneInstance)),
                new IconPayload(BitmapFontIcon.CrossWorld),
                new TextPayload($"{ev.World.Name} {Localization.WasKilled} {Utils.FormatTimeSpan(ev.KilledAt)}".TrimEnd()),
            ]);
        }

        Dalamud.ChatGui.Print(new XivChatEntry
        {
            Name = "Faloop",
            Message = new SeString(payloads),
            Type = Enum.GetValues<XivChatType>()[channel],
        });
    }

    private static void OnAny(string name, SocketIOResponse response)
    {
        DalamudLog.Log.Debug("Event {Name} = {Message}", name, response);
    }

    private static void OnPing()
    {
        DalamudLog.Log.Debug("Ping");
    }

    private static void OnPong(TimeSpan span)
    {
        DalamudLog.Log.Debug("Pong: {Span}", span);
    }

    public void Connect()
    {
        if (string.IsNullOrWhiteSpace(Config.FaloopUsername) || string.IsNullOrWhiteSpace(Config.FaloopPassword))
        {
            Divination.Chat.Print(Localization.AccountNotSet);
            return;
        }

        // restart the connection loop, e.g. when the account is changed
        connectionCancellation?.Cancel();
        connectionCancellation?.Dispose();
        connectionCancellation = new CancellationTokenSource();
        var cancellationToken = connectionCancellation.Token;
        Task.Run(() => RunConnectionLoop(Config.FaloopUsername, Config.FaloopPassword, cancellationToken), cancellationToken);
    }

    /// <summary>
    /// Keeps the connection to Faloop. On every disconnection, logs in again and connects with a new Socket.IO client,
    /// because the client's own reconnection may never get the namespace connection accepted again.
    /// </summary>
    private async Task RunConnectionLoop(string username, string password, CancellationToken cancellationToken)
    {
        var delay = MinReconnectDelay;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                DalamudLog.Log.Debug("Connecting to Faloop");
                if (!await session.LoginAsync(username, password))
                {
                    // do not retry with credentials that are rejected
                    DalamudLog.Log.Warning("Failed to log in to Faloop");
                    return;
                }

                disconnected = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                await socket.Connect(session, ConnectTimeout, cancellationToken);
                DalamudLog.Log.Debug("Connected to Faloop");
                delay = MinReconnectDelay;

                await disconnected.Task.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                DalamudLog.Log.Error(exception, "connect failed");
            }

            DalamudLog.Log.Information("Reconnecting to Faloop in {Delay}", delay);
            try
            {
                await Task.Delay(delay, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            delay = TimeSpan.FromTicks(Math.Min(delay.Ticks * 2, MaxReconnectDelay.Ticks));
        }
    }

    public void EmitMockData()
    {
        Task.Run(async () =>
        {
            try
            {
                OnMobReport(MockData.SpawnMobReport);
                await Task.Delay(300000);
                OnMobReport(MockData.DeathMobReport);
            }
            catch (Exception exception)
            {
                DalamudLog.Log.Error(exception, nameof(EmitMockData));
            }
        });
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        connectionCancellation?.Cancel();
        connectionCancellation?.Dispose();
        socket.Dispose();
        Ui.Dispose();
        Dalamud.PluginInterface.UiBuilder.Draw -= Ui.Draw;
        Dalamud.ClientState.Login -= OnLogin;
    }

    public string MainCommandPrefix => "/faloop";

    public ConfigWindow<PluginConfig> CreateConfigWindow()
    {
        return new PluginConfigWindow();
    }
}
