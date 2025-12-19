using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Command;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;
using Divination.AetheryteLinkInChat.Config;
using Divination.AetheryteLinkInChat.Ipc;
using Divination.AetheryteLinkInChat.Payloads;
using Divination.AetheryteLinkInChat.Solver;

namespace Divination.AetheryteLinkInChat;

public class AetheryteLinkInChat : DivinationPlugin<AetheryteLinkInChat, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private const uint AetheryteLinkCommandId = 0;
    private const uint LifestreamLinkCommandId = 1;
    private const string TeleportGcCommand = "/teleportgc";

    private readonly DalamudLinkPayload aetheryteLinkPayload;
    private readonly DalamudLinkPayload lifestreamLinkPayload;
    private readonly AetheryteSolver solver;
    private readonly Teleporter teleporter;
    private readonly IpcProvider ipcProvider;

    private CancellationTokenSource? lifestreamCancellation;

    public AetheryteLinkInChat(IDalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        aetheryteLinkPayload = Dalamud.ChatGui.AddChatLinkHandler(AetheryteLinkCommandId, HandleAetheryteLink);
        lifestreamLinkPayload = Dalamud.ChatGui.AddChatLinkHandler(LifestreamLinkCommandId, HandleLifestreamLink);
        solver = new AetheryteSolver(Dalamud.DataManager);
        teleporter = new Teleporter(Dalamud.Condition, Dalamud.AetheryteList, Divination.Chat, Dalamud.CommandManager, Dalamud.ObjectTable, Dalamud.PluginInterface, Dalamud.ToastGui, Dalamud.Framework, Config);
        ipcProvider = new IpcProvider(pluginInterface, Dalamud.ObjectTable, teleporter, solver, Dalamud.DataManager);

        Dalamud.ChatGui.ChatMessage += OnChatReceived;
        Dalamud.CommandManager.AddHandler(TeleportGcCommand,
            new CommandInfo(OnTeleportGcCommand)
            {
                HelpMessage = Localization.TeleportGcHelpMessage,
            });
    }

    public string MainCommandPrefix => "/alic";

    public ConfigWindow<PluginConfig> CreateConfigWindow()
    {
        return new PluginConfigWindow();
    }

    private void OnChatReceived(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        try
        {
            AppendNearestAetheryteLink(ref message);
        }
        catch (Exception exception)
        {
            DalamudLog.Log.Error(exception, nameof(OnChatReceived));
        }
    }

    private void AppendNearestAetheryteLink(ref SeString message)
    {
        var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
        if (mapLink == default)
        {
            return;
        }

        // 最短のテレポート経路を計算する
        var paths = solver.CalculateTeleportPathsForMapLink(mapLink).ToList();
        if (paths.Count == 0)
        {
            DalamudLog.Log.Debug("AppendNearestAetheryteLink: paths.Count == 0");
            return;
        }

        // ワールド間テレポの経路を追加する
        if (Config.ConsiderTeleportsToOtherWorlds)
        {
            solver.AppendGrandCompanyAetheryte(paths,
                (uint)Enum.GetValues<GrandCompanyAetheryte>()[Config.PreferredGrandCompanyAetheryte],
                message,
                Dalamud.ObjectTable.LocalPlayer?.CurrentWorld.Value,
                Dalamud.ClientState.TerritoryType);
        }

        var payloads = new List<Payload>();
        foreach (var (index, path) in paths.Select((x, i) => (i, x)))
        {
            switch (path)
            {
                // テレポ可能なエーテライト
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: true } aetheryte:
                    payloads.AddRange([
                        new IconPayload(BitmapFontIcon.Aetheryte),
                        new UIForegroundPayload(069),
                        ..SeString.TextArrowPayloads,
                        aetheryteLinkPayload,
                        new TextPayload(aetheryte.Aetheryte.PlaceName.Value.Name.ExtractText()),
                        new AetherytePayload(aetheryte.Aetheryte).ToRawPayload(),
                        RawPayload.LinkTerminator,
                        UIForegroundPayload.UIForegroundOff,
                    ]);
                    break;
                // 仮設エーテライト・都市内エーテライト
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: false } aetheryte:
                    payloads.AddRange([
                        new IconPayload(BitmapFontIcon.Aethernet),
                        new TextPayload(aetheryte.Aetheryte.AethernetName.Value.Name.ExtractText()),
                    ]);
                    break;
                // マップ境界
                case BoundaryTeleportPath boundary:
                    payloads.AddRange([
                        new IconPayload(BitmapFontIcon.FlyZone),
                        new TextPayload(boundary.ConnectedMarker.PlaceNameSubtext.Value.Name.ExtractText()),
                    ]);
                    break;
                // ワールド間テレポ
                case WorldTeleportPath world:
                    payloads.AddRange([
                        new IconPayload(BitmapFontIcon.Aetheryte),
                        new UIForegroundPayload(069),
                        aetheryteLinkPayload,
                        new TextPayload(world.Aetheryte.PlaceName.Value.Name.ExtractText()),
                        new AetherytePayload(world.Aetheryte).ToRawPayload(),
                        RawPayload.LinkTerminator,
                        UIForegroundPayload.UIForegroundOff,
                        new TextPayload($" {SeIconChar.ArrowRight.ToIconString()} "),
                        new IconPayload(BitmapFontIcon.CrossWorld),
                        new TextPayload(world.World.Name.ExtractText()),
                    ]);
                    break;
            }

            if (index != paths.Count - 1)
            {
                payloads.Add(new TextPayload($" {SeIconChar.ArrowRight.ToIconString()} "));
            }
        }

        if (Config.EnableLifestreamIntegration && teleporter.IsLifestreamAvailable())
        {
            var world = solver.DetectWorld(message, Dalamud.ObjectTable.LocalPlayer?.CurrentWorld.Value);

            payloads.AddRange([
                new TextPayload(" ["),
                new IconPayload(BitmapFontIcon.Aetheryte),
                new UIForegroundPayload(069),
                ..SeString.TextArrowPayloads,
                lifestreamLinkPayload,
                new TextPayload("Lifestream"),
                new LifestreamPayload(mapLink, world?.RowId).ToRawPayload(),
                RawPayload.LinkTerminator,
                UIForegroundPayload.UIForegroundOff,
                new TextPayload("]"),
            ]);
        }

        if (payloads.Count > 0)
        {
            if (Config.DisplayLineBreak)
            {
                payloads.Insert(0, new TextPayload("\n"));
                message.Payloads.AddRange(payloads);
            }
            else if (Config.DisplayLinkAtEnd)
            {
                message.Payloads.AddRange(payloads);
            }
            else
            {
                var mapIndex = message.Payloads.FindIndex(p => p.GetType() == typeof(MapLinkPayload)) + 8;
                message.Payloads.InsertRange(mapIndex, payloads);
            }
        }
    }

    private void HandleAetheryteLink(uint _, SeString link)
    {
        var payload = link.Payloads.OfType<RawPayload>().Select(AetherytePayload.Parse).FirstOrDefault(x => x != default);
        if (payload == default)
        {
            DalamudLog.Log.Error("HandleAetheryteLink: aetheryte ({Text}) == null", link.ToString());
            return;
        }

        DalamudLog.Log.Debug("HandleAetheryteLink: parsed payload = {Payload}", payload);
        teleporter.TeleportToAetheryte(payload.Aetheryte).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DalamudLog.Log.Debug("HandleAetheryteLink: TeleportToAetheryte executed: {Result}", task.Result);
            }
            else
            {
                DalamudLog.Log.Warning(task.Exception, "HandleAetheryteLink: TeleportToAetheryte failed");
            }
        });
    }

    private void HandleLifestreamLink(uint _, SeString link)
    {
        if (!teleporter.IsLifestreamAvailable())
        {
            Divination.Chat.PrintError(Localization.LifestreamUnavailable);
            return;
        }

        var payload = link.Payloads.OfType<RawPayload>().Select(LifestreamPayload.Parse).FirstOrDefault(x => x != default);
        if (payload == default)
        {
            DalamudLog.Log.Error("HandleLifestreamLink: combined aetheryte ({Text}) == null", link.ToString());
            return;
        }

        DalamudLog.Log.Debug("HandleLifestreamLink: parsed payload = {Payload}", payload);

        var paths = solver.CalculateTeleportPathsForMapLink(payload.MapLink).ToList();
        if (paths.Count == 0)
        {
            DalamudLog.Log.Debug("HandleLifestreamLink: paths.Count == 0");
            return;
        }

        // cancel previous teleportation
        if (lifestreamCancellation != default)
        {
            lifestreamCancellation.Cancel();
            lifestreamCancellation.Dispose();
        }

        lifestreamCancellation = new CancellationTokenSource();
        teleporter.TeleportToPaths(paths, payload.World, lifestreamCancellation.Token).ContinueWith((task) =>
        {
            if (task.IsCanceled)
            {
                return;
            }

            if (task.Exception != default)
            {
                DalamudLog.Log.Error(task.Exception, "HandleLifestreamLink: TeleportToPaths failed");
            }
            else
            {
                DalamudLog.Log.Debug("HandleLifestreamLink: TeleportToPaths: {Result}", task.Result);
            }
        });
    }

    private void OnTeleportGcCommand(string command, string arguments)
    {
        var aetheryteId = (uint)Enum.GetValues<GrandCompanyAetheryte>()[Config.PreferredGrandCompanyAetheryte];
        if (aetheryteId == default)
        {
            return;
        }

        var aetheryte = solver.GetAetheryteById(aetheryteId);
        if (!aetheryte.HasValue)
        {
            return;
        }

        teleporter.TeleportToAetheryte(aetheryte.Value).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DalamudLog.Log.Debug("OnTeleportGcCommand: TeleportToAetheryte executed: {Result}", task.Result);
            }
            else
            {
                DalamudLog.Log.Warning(task.Exception, "OnTeleportGcCommand: TeleportToAetheryte failed");
            }
        });
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        Dalamud.ChatGui.RemoveChatLinkHandler();
        Dalamud.ChatGui.ChatMessage -= OnChatReceived;
        Dalamud.CommandManager.RemoveHandler(TeleportGcCommand);
        teleporter.Dispose();
        lifestreamCancellation?.Dispose();
        ipcProvider.Dispose();
    }
}
