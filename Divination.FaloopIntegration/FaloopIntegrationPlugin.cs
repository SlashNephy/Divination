using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Gui.PartyFinder.Types;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;
using Divination.FaloopIntegration.Config;
using Divination.FaloopIntegration.Faloop;
using Divination.FaloopIntegration.Faloop.Model;
using Lumina.Excel.GeneratedSheets;
using SocketIOClient;

namespace Divination.FaloopIntegration;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class FaloopIntegrationPlugin : DivinationPlugin<FaloopIntegrationPlugin, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private readonly FaloopSocketIOClient socket = new();

    public FaloopIntegrationPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
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
        Dalamud.PartyFinderGui.ReceiveListing += OnReceiveListing;

        Connect();
    }

    private void OnConnected()
    {
        Divination.Chat.Print("Faloop に接続しました！");
    }

    private void OnDisconnected(string cause)
    {
        Divination.Chat.Print("Faloop から切断されました。");
        PluginLog.Warning("Disconnected = {Cause}", cause);
    }

    private static void OnError(string error)
    {
        PluginLog.Error("Error = {Error}", error);
    }

    private void OnMobReport(MobReportData data)
    {
        var mob = Dalamud.DataManager.GetExcelSheet<BNpcName>()?.GetRow(data.MobId);
        if (mob == default)
        {
            PluginLog.Debug("OnMobReport: mob == null");
            return;
        }

        var rank = FaloopEmbedData.Mobs.Find(x => x.Id == data.MobId)?.Rank;
        if (rank == default)
        {
            PluginLog.Debug("OnMobReport: rank == null");
            return;
        }

        var world = Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(data.WorldId);
        var dataCenter = world?.DataCenter?.Value;
        if (world == default || dataCenter == default)
        {
            PluginLog.Debug("OnMobReport: world == null || dataCenter == null");
            return;
        }

        var currentWorld = Dalamud.ClientState.LocalPlayer?.CurrentWorld.GameData;
        var currentDataCenter = currentWorld?.DataCenter?.Value;
        if (currentWorld == default || currentDataCenter == default)
        {
            PluginLog.Debug("OnMobReport: currentWorld == null || currentDataCenter == null");
            return;
        }

        var config = rank switch
        {
            "S" => Config.RankS,
            "A" => Config.RankA,
            "B" => Config.RankB,
            "F" => Config.Fate,
            _ => default,
        };
        if (config == default)
        {
            PluginLog.Debug("OnMobReport: config == null");
            return;
        }

        if (config.DisableInDuty && Dalamud.Condition[ConditionFlag.BoundByDuty])
        {
            PluginLog.Debug("OnMobReport: in duty");
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
                PluginLog.Verbose("OnMobReport: unmatched");
                return;
        }

        switch (data.Action)
        {
            case "spawn" when config.EnableSpawnReport:
                OnSpawnMobReport(data, mob, world, config.Channel);
                PluginLog.Verbose("OnMobReport: OnSpawnMobReport");
                break;
            case "death" when config.EnableDeathReport:
                OnDeathMobReport(data, mob, world, config.Channel);
                PluginLog.Verbose("OnMobReport: OnDeathMobReport");
                break;
        }
    }

    private void OnSpawnMobReport(MobReportData data, BNpcName mob, World world, int channel)
    {
        var spawn = data.Data.Deserialize<MobReportData.Spawn>();
        if (spawn == default)
        {
            PluginLog.Debug("OnSpawnMobReport: spawn == null");
            return;
        }

        var payloads = new List<Payload>
        {
            new TextPayload($"{SeIconChar.BoxedLetterS.ToIconString()} {mob.Singular.RawString} "),
        };

        var mapLink = CreateMapLink(spawn.ZoneId, spawn.ZonePoiIds.First(), data.ZoneInstance);
        if (mapLink != default)
        {
            payloads.AddRange(mapLink.Payloads);
        }

        payloads.AddRange(new Payload[]
        {
            new IconPayload(BitmapFontIcon.CrossWorld),
            new TextPayload($"{world.Name} が湧きました。({FormatTimeSpan(spawn.Timestamp)})"),
        });

        Dalamud.ChatGui.PrintChat(new XivChatEntry
        {
            Name = "Faloop",
            Message = new SeString(payloads),
            Type = Enum.GetValues<XivChatType>()[channel],
        });
    }

    private void OnDeathMobReport(MobReportData data, BNpcName mob, World world, int channel)
    {
        var death = data.Data.Deserialize<MobReportData.Death>();
        if (death == default)
        {
            PluginLog.Debug("OnDeathMobReport: death == null");
            return;
        }

        Dalamud.ChatGui.PrintChat(new XivChatEntry
        {
            Name = "Faloop",
            Message = new SeString(new List<Payload>
            {
                new TextPayload($"{SeIconChar.BoxedLetterS.ToIconString()} {mob.Singular.RawString}"),
                new IconPayload(BitmapFontIcon.CrossWorld),
                new TextPayload($"{world.Name} が討伐されました。({FormatTimeSpan(death.StartedAt)})"),
            }),
            Type = Enum.GetValues<XivChatType>()[channel],
        });
    }

    private SeString? CreateMapLink(uint zoneId, int zonePoiId, int? instance)
    {
        var zone = Dalamud.DataManager.GetExcelSheet<TerritoryType>()?.GetRow(zoneId);
        var map = zone?.Map.Value;
        if (zone == default || map == default)
        {
            PluginLog.Debug("CreateMapLink: zone == null || map == null");
            return default;
        }

        var location = FaloopEmbedData.ZonePois.Find(x => x.Id == zonePoiId);
        if (location == default)
        {
            PluginLog.Debug("CreateMapLink: location == null");
            return default;
        }

        var n = 41 / (map.SizeFactor / 100.0);
        var loc = location.Location.Split(new[] { ',' }, 2)
            .Select(int.Parse)
            .Select(x => x / 2048.0 * n + 1)
            .Select(x => Math.Round(x, 1))
            .Select(x => (float)x)
            .ToList();

        var mapLink = SeString.CreateMapLink(zone.RowId, zone.Map.Row, loc[0], loc[1]);

        var instanceIcon = GetInstanceIcon(instance);
        return instanceIcon != default ? mapLink.Append(instanceIcon) : mapLink;
    }

    private static TextPayload? GetInstanceIcon(int? instance)
    {
        switch (instance)
        {
            case 1:
                return new TextPayload(SeIconChar.Instance1.ToIconString());
            case 2:
                return new TextPayload(SeIconChar.Instance2.ToIconString());
            case 3:
                return new TextPayload(SeIconChar.Instance3.ToIconString());
            default:
                return default;
        }
    }

    private static string FormatTimeSpan(DateTime time)
    {
        var span = DateTime.UtcNow - time;
        var builder = new StringBuilder();
        if (span.Days > 0)
        {
            builder.Append($"{span.Days}日前");
        }
        else if (span.Hours > 0)
        {
            builder.Append($"{span.Hours}時間前");
        }
        else if (span.Minutes > 0)
        {
            builder.Append($"{span.Minutes}分前");
        }
        else if (span.Seconds > 10)
        {
            builder.Append($"{span.Seconds}秒前");
        }
        else
        {
            builder.Append("たった今");
        }

        return builder.ToString();
    }

    private static void OnAny(string name, SocketIOResponse response)
    {
        PluginLog.Debug("Event {Name} = {Message}", name, response);
    }

    private static void OnReconnected(int count)
    {
        PluginLog.Debug("Reconnected {N}", count);
    }

    private static void OnReconnectError(Exception exception)
    {
        PluginLog.Error(exception, "Reconnect error");
    }

    private static void OnReconnectAttempt(int count)
    {
        PluginLog.Debug("Reconnect attempt {N}", count);
    }

    private static void OnReconnectFailed()
    {
        PluginLog.Debug("Reconnect failed");
    }

    private static void OnPing()
    {
        PluginLog.Debug("Ping");
    }

    private static void OnPong(TimeSpan span)
    {
        PluginLog.Debug("Pong: {Span}", span);
    }

    private void OnReceiveListing(PartyFinderListing listing, PartyFinderListingEventArgs args)
    {
        if (listing.Category != DutyCategory.TheHunt)
        {
            return;
        }

        if (Dalamud.PartyList.Length > 1 || listing.Name.TextValue == Dalamud.ClientState.LocalPlayer?.Name.TextValue)
        {
            return;
        }

        Dalamud.ChatGui.PrintChat(new XivChatEntry
        {
            Name = "パーティ募集",
            Message = new SeString(new List<Payload>
            {
                new TextPayload($"モブハントに募集があります。 {listing.Name.TextValue}"),
            }),
            Type = Enum.GetValues<XivChatType>()[Config.PartyFinder.Channel],
        });

        PluginLog.Debug("ReceiveListing: {Name}, {Description}", listing.Name.TextValue, listing.Description.TextValue);
    }

    public void Connect()
    {
        if (string.IsNullOrWhiteSpace(Config.FaloopUsername) || string.IsNullOrWhiteSpace(Config.FaloopPassword))
        {
            Divination.Chat.Print("Faloop のログイン情報が設定されていません。/faloop から設定できます。");
            return;
        }

        Task.Run(async () =>
        {
            try
            {
                await socket.Connect(Config.FaloopUsername, Config.FaloopPassword);
            }
            catch (Exception exception)
            {
                PluginLog.Error(exception, "connect failed");
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
                await Task.Delay(3000);
                OnMobReport(MockData.DeathMobReport);
            }
            catch (Exception exception)
            {
                PluginLog.Error(exception, nameof(EmitMockData));
            }
        });
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        socket.Dispose();
        Dalamud.PartyFinderGui.ReceiveListing -= OnReceiveListing;
    }

    public string MainCommandPrefix => "/faloop";

    public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();
}
