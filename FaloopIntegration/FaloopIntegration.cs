using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;
using Divination.FaloopIntegration.Config;
using Divination.FaloopIntegration.Faloop;
using Divination.FaloopIntegration.Faloop.Model;
using Divination.FaloopIntegration.Ipc;
using Divination.FaloopIntegration.Ui;
using Lumina.Excel.GeneratedSheets;
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

    public FaloopIntegration(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();

        socket.OnConnected += OnConnected;
        socket.OnDisconnected += OnDisconnected;
        socket.OnError += OnError;
        socket.OnMobReport += OnMobReport;
        socket.OnAny += OnAny;
        socket.OnReconnected += OnReconnected;
        socket.OnReconnectError += OnReconnectError;
        socket.OnReconnectAttempt += OnReconnectAttempt;
        socket.OnReconnectFailed += OnReconnectFailed;
        socket.OnPing += OnPing;
        socket.OnPong += OnPong;

        var ipc = new AetheryteLinkInChatIpc(pluginInterface, Divination.Chat);
        Ui = new ActiveMobUi(ipc, Divination.Chat)
        {
            IsDrawing = Config.EnableActiveMobUi
        };
        Dalamud.PluginInterface.UiBuilder.Draw += Ui.Draw;

        Connect();
        CleanSpawnHistories();
    }

    private void OnConnected()
    {
        Divination.Chat.Print(Localization.Connected);
    }

    private void OnDisconnected(string cause)
    {
        Divination.Chat.Print(Localization.Disconnected);
        DalamudLog.Log.Warning("Disconnected = {Cause}", cause);
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
                    if (!FaloopEmbedData.Locations.TryGetValue(spawn.ZonePoiIds?.FirstOrDefault() ?? default, out var location))
                    {
                        DalamudLog.Log.Debug("OnMobReport: location == null");
                    }

                    var ev = new MobSpawnEvent(mobData.BNpcId, worldId, spawn.ZoneId, data.ZoneInstance, mobData.Rank, spawn.Timestamp, spawn.Reporters?.FirstOrDefault()?.Name, location);
                    OnMobSpawn(ev, config.Channel);
                    DalamudLog.Log.Verbose("OnMobReport: OnSpawnMobReport");
                    break;
                }
            case MobReportActions.SpawnLocation when config.EnableSpawnReport:
                {
                    var spawn = JsonSerializer.Deserialize<MobReportData.SpawnLocation>(data.Data) ?? throw new InvalidOperationException("invalid spawn location data");
                    if (!FaloopEmbedData.Locations.TryGetValue(spawn.ZonePoiId, out var location))
                    {
                        DalamudLog.Log.Debug("OnMobReport: location == null");
                    }

                    var previous = Config.SpawnStates.FirstOrDefault(x => x.MobId == mobData.BNpcId && x.WorldId == worldId);
                    var ev = new MobSpawnEvent(mobData.BNpcId, worldId, spawn.ZoneId, data.ZoneInstance, mobData.Rank, previous?.SpawnedAt ?? DateTime.UtcNow, previous?.Reporter, location);
                    OnMobSpawn(ev, config.Channel);
                    break;
                }
            case MobReportActions.SpawnRelease when config.EnableSpawnReport:
                {
                    var spawn = JsonSerializer.Deserialize<MobReportData.SpawnRelease>(data.Data) ?? throw new InvalidOperationException("invalid spawn release data");
                    var previous = Config.SpawnStates.FirstOrDefault(x => x.MobId == mobData.BNpcId && x.WorldId == worldId);
                    if (previous == default)
                    {
                        DalamudLog.Log.Debug("OnMobReport: previous == null");
                        break;
                    }
                    var ev = new MobSpawnEvent(mobData.BNpcId, worldId, previous.TerritoryTypeId, data.ZoneInstance, mobData.Rank, spawn.Timestamp, previous.Reporter, previous.Location);
                    OnMobSpawn(ev, config.Channel);
                    break;
                }
            case MobReportActions.Death when config.EnableDeathReport:
                var death = JsonSerializer.Deserialize<MobReportData.Death>(data.Data) ?? throw new InvalidOperationException("invalid death data");
                OnMobDeath(new MobDeathEvent(mobData.BNpcId, worldId, data.ZoneInstance, mobData.Rank, death.StartedAt), config.Channel, config.SkipOrphanReport);
                DalamudLog.Log.Verbose("OnMobReport: OnDeathMobReport");
                break;
        }
    }

    private bool CheckSpawnNotificationCondition(PluginConfig.PerRankConfig config, uint worldId, GameExpansion expansion)
    {
        var world = Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(worldId);
        var dataCenter = world?.DataCenter?.Value;
        if (world == default || dataCenter == default)
        {
            DalamudLog.Log.Debug("OnMobReport: world == null || dataCenter == null");
            return false;
        }

        var currentWorld = Dalamud.ClientState.LocalPlayer?.CurrentWorld.GameData;
        var currentDataCenter = currentWorld?.DataCenter?.Value;
        if (currentWorld == default || currentDataCenter == default)
        {
            // TODO
            DalamudLog.Log.Debug("OnMobReport: currentWorld == null || currentDataCenter == null");
            return false;
        }

        if (!config.Expansions.TryGetValue(expansion, out var value) || !value)
        {
            DalamudLog.Log.Debug("OnMobReport: MajorPatches");
            return false;
        }

        if (config.DisableInDuty && Dalamud.Condition[ConditionFlag.BoundByDuty])
        {
            DalamudLog.Log.Debug("OnMobReport: in duty");
            return false;
        }

        switch ((Jurisdiction)config.Jurisdiction)
        {
            case Jurisdiction.All:
            case Jurisdiction.Region when dataCenter.Region == currentDataCenter.Region || (config.IncludeOceaniaDataCenter && dataCenter.Region == 4):
            case Jurisdiction.DataCenter when dataCenter.RowId == currentDataCenter.RowId:
            case Jurisdiction.World when world.RowId == currentWorld.RowId:
                return true;
            default:
                DalamudLog.Log.Verbose("OnMobReport: unmatched");
                return false;
        }
    }

    private void OnMobSpawn(MobSpawnEvent ev, int channel)
    {
        Ui.OnMobSpawn(ev);

        Config.SpawnStates.Add(ev);
        Dalamud.PluginInterface.SavePluginConfig(Config);

        var payloads = new List<Payload>();
        if (Config.EnableSimpleReports)
        {
            payloads.Add(new TextPayload($"{SeIconChar.BoxedPlus.ToIconString()}"));
        }
        payloads.Add(Utils.GetRankIcon(ev.Rank));
        payloads.Add(new TextPayload($" {ev.Mob.Singular.RawString} "));

        // append MapLink only if pop location is known
        if (ev.Coordinates.HasValue)
        {
            var mapLink = Utils.CreateMapLink(ev.TerritoryType, ev.Map, ev.Coordinates.Value, ev.ZoneInstance);
            payloads.AddRange(mapLink.Payloads);
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
            Type = Enum.GetValues<XivChatType>()[channel],
        });
    }

    private void OnMobDeath(MobDeathEvent ev, int channel, bool skipOrphanReport)
    {
        Ui.OnMobDeath(ev);

        if (skipOrphanReport && Config.SpawnStates.RemoveAll(x => x.Id == ev.Id) == 0)
        {
            DalamudLog.Log.Debug("OnDeathMobReport: skipOrphanReport");
            return;
        }
        Dalamud.PluginInterface.SavePluginConfig(Config);

        var payloads = new List<Payload>();
        if (Config.EnableSimpleReports)
        {
            payloads.AddRange(
            [
                new TextPayload($"{SeIconChar.Cross.ToIconString()}"),
                new TextPayload($" {ev.Mob.Singular.RawString}"),
                new IconPayload(BitmapFontIcon.CrossWorld),
                new TextPayload($"{ev.World.Name}".TrimEnd()),
            ]);
        }
        else
        {
            payloads.AddRange(
            [
                Utils.GetRankIcon(ev.Rank),
                new TextPayload($" {ev.Mob.Singular.RawString}"),
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

    private static void OnReconnected(int count)
    {
        DalamudLog.Log.Debug("Reconnected {N}", count);
    }

    private static void OnReconnectError(Exception exception)
    {
        DalamudLog.Log.Error(exception, "Reconnect error");
    }

    private static void OnReconnectAttempt(int count)
    {
        DalamudLog.Log.Debug("Reconnect attempt {N}", count);
    }

    private static void OnReconnectFailed()
    {
        DalamudLog.Log.Debug("Reconnect failed");
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

        Task.Run(async () =>
        {
            try
            {
                if (await session.LoginAsync(Config.FaloopUsername, Config.FaloopPassword))
                {
                    await socket.Connect(session);
                }
            }
            catch (Exception exception)
            {
                DalamudLog.Log.Error(exception, "connect failed");
            }
        });
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

    private void CleanSpawnHistories()
    {
        Config.SpawnStates.RemoveAll(x => DateTime.UtcNow - x.SpawnedAt > TimeSpan.FromHours(1));
        Dalamud.PluginInterface.SavePluginConfig(Config);
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        socket.Dispose();
        Ui.Dispose();
        Dalamud.PluginInterface.UiBuilder.Draw -= Ui.Draw;
    }

    public string MainCommandPrefix => "/faloop";

    public ConfigWindow<PluginConfig> CreateConfigWindow()
    {
        return new PluginConfigWindow();
    }
}
