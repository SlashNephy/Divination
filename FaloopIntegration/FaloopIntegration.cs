using System;
using System.Collections.Generic;
using System.Linq;
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
    public readonly ActiveMobUi Ui = new();

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

        Ui.IsDrawing = Config.EnableActiveMobUi;
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
        var mob = Dalamud.DataManager.GetExcelSheet<BNpcName>()?.GetRow(data.MobId);
        if (mob == default)
        {
            DalamudLog.Log.Debug("OnMobReport: mob == null");
            return;
        }

        var mobData = session.EmbedData.Mobs.FirstOrDefault(x => x.Id == data.MobId);
        if (mobData == default)
        {
            DalamudLog.Log.Debug("OnMobReport: mobData == null");
            return;
        }

        var world = Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(data.WorldId);
        var dataCenter = world?.DataCenter?.Value;
        if (world == default || dataCenter == default)
        {
            DalamudLog.Log.Debug("OnMobReport: world == null || dataCenter == null");
            return;
        }

        var currentWorld = Dalamud.ClientState.LocalPlayer?.CurrentWorld.GameData;
        var currentDataCenter = currentWorld?.DataCenter?.Value;
        if (currentWorld == default || currentDataCenter == default)
        {
            DalamudLog.Log.Debug("OnMobReport: currentWorld == null || currentDataCenter == null");
            return;
        }

        var config = mobData.Rank switch
        {
            "S" => Config.RankS,
            "F" => Config.Fate,
            _ => default,
        };
        if (config == default)
        {
            DalamudLog.Log.Debug("OnMobReport: config == null");
            return;
        }

        if (!config.MajorPatches.TryGetValue(mobData.Version, out var value) || !value)
        {
            DalamudLog.Log.Debug("OnMobReport: MajorPatches");
            return;
        }

        if (config.DisableInDuty && Dalamud.Condition[ConditionFlag.BoundByDuty])
        {
            DalamudLog.Log.Debug("OnMobReport: in duty");
            return;
        }

        switch ((Jurisdiction)config.Jurisdiction)
        {
            case Jurisdiction.All:
            case Jurisdiction.Region when dataCenter.Region == currentDataCenter.Region:
            case Jurisdiction.DataCenter when dataCenter.RowId == currentDataCenter.RowId:
            case Jurisdiction.World when world.RowId == currentWorld.RowId:
                break;
            default:
                DalamudLog.Log.Verbose("OnMobReport: unmatched");
                return;
        }

        switch (data.Action)
        {
            case "spawn" when config.EnableSpawnReport:
                OnMobSpawn(new MobSpawnEvent(data, mob, world, mobData.Rank), config.Channel);
                DalamudLog.Log.Verbose("OnMobReport: OnSpawnMobReport");
                break;
            case "death" when config.EnableDeathReport:
                OnMobDeath(new MobDeathEvent(data, mob, world, mobData.Rank), config.Channel, config.SkipOrphanReport);
                DalamudLog.Log.Verbose("OnMobReport: OnDeathMobReport");
                break;
        }
    }

    private void OnMobSpawn(MobSpawnEvent ev, int channel)
    {
        Ui.OnMobSpawn(ev);

        Config.SpawnHistories.Add(new PluginConfig.SpawnHistory
        {
            MobId = ev.Data.MobId,
            WorldId = ev.Data.WorldId,
            ZoneInstance = ev.Data.ZoneInstance,
            At = ev.Spawn.Timestamp,
        });
        Dalamud.PluginInterface.SavePluginConfig(Config);

        var payloads = new List<Payload>();
        if (Config.EnableSimpleReports)
        {
            payloads.Add(new TextPayload($"{SeIconChar.BoxedPlus.ToIconString()}"));
        }
        payloads.Add(Utils.GetRankIcon(ev.Rank));
        payloads.Add(new TextPayload($" {ev.Mob.Singular.RawString} "));

        // append MapLink only if pop location is known
        if (ev.Spawn.ZonePoiIds?.Count > 0)
        {
            var mapLink = CreateMapLink(ev.Spawn.ZoneId, ev.Spawn.ZonePoiIds.First(), ev.Data.ZoneInstance);
            if (mapLink != default)
            {
                payloads.AddRange(mapLink.Payloads);
            }
        }

        payloads.Add(new IconPayload(BitmapFontIcon.CrossWorld));

        if (Config.EnableSimpleReports)
        {
            payloads.Add(new TextPayload($"{ev.World.Name}".TrimEnd()));
        }
        else
        {
            payloads.Add(new TextPayload($"{ev.World.Name} {Localization.HasSpawned} {Utils.FormatTimeSpan(ev.Spawn.Timestamp)}".TrimEnd()));
        }

        Dalamud.ChatGui.Print(new XivChatEntry
        {
            Name = ev.Spawn.Reporters?.FirstOrDefault()?.Name ?? "Faloop",
            Message = new SeString(payloads),
            Type = Enum.GetValues<XivChatType>()[channel],
        });
    }

    private void OnMobDeath(MobDeathEvent ev, int channel, bool skipOrphanReport)
    {
        Ui.OnMobDeath(ev);

        if (skipOrphanReport && Config.SpawnHistories.RemoveAll(x => x.MobId == ev.Data.MobId && x.WorldId == ev.Data.WorldId && x.ZoneInstance == ev.Data.ZoneInstance) == 0)
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
                new TextPayload($"{ev.World.Name} {Localization.WasKilled} {Utils.FormatTimeSpan(ev.Death.StartedAt)}".TrimEnd()),
            ]);
        }

        Dalamud.ChatGui.Print(new XivChatEntry
        {
            Name = "Faloop",
            Message = new SeString(payloads),
            Type = Enum.GetValues<XivChatType>()[channel],
        });
    }

    private SeString? CreateMapLink(uint zoneId, int zonePoiId, int? instance)
    {
        var zone = Dalamud.DataManager.GetExcelSheet<TerritoryType>()?.GetRow(zoneId);
        var map = zone?.Map.Value;
        if (zone == default || map == default)
        {
            DalamudLog.Log.Debug("CreateMapLink: zone == null || map == null");
            return default;
        }

        var location = session.EmbedData.ZoneLocations.FirstOrDefault(x => x.Id == zonePoiId);
        if (location == default)
        {
            DalamudLog.Log.Debug("CreateMapLink: location == null");
            return default;
        }

        var n = 41 / (map.SizeFactor / 100.0);
        var loc = location.Location.Split([','], 2)
            .Select(int.Parse)
            .Select(x => x / 2048.0 * n + 1)
            .Select(x => Math.Round(x, 1))
            .Select(x => (float)x)
            .ToList();

        var mapLink = SeString.CreateMapLink(zone.RowId, zone.Map.Row, loc[0], loc[1]);

        var instanceIcon = Utils.GetInstanceIcon(instance);
        return instanceIcon != default ? mapLink.Append(instanceIcon) : mapLink;
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
        Config.SpawnHistories.RemoveAll(x => DateTime.UtcNow - x.At > TimeSpan.FromHours(1));
        Dalamud.PluginInterface.SavePluginConfig(Config);
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        socket.Dispose();
        Dalamud.PluginInterface.UiBuilder.Draw -= Ui.Draw;
    }

    public string MainCommandPrefix => "/faloop";

    public ConfigWindow<PluginConfig> CreateConfigWindow()
    {
        return new PluginConfigWindow();
    }
}
