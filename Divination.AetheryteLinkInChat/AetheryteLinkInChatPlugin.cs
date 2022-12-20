using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;
using Divination.AetheryteLinkInChat.Config;
using Divination.AetheryteLinkInChat.Localize;

namespace Divination.AetheryteLinkInChat;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class AetheryteLinkInChatPlugin : DivinationPlugin<AetheryteLinkInChatPlugin, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private const uint LinkCommandId = 0;

    private readonly DalamudLinkPayload linkPayload;
    private readonly AetheryteSolver solver;
    private readonly Teleporter teleporter;
    private readonly Regex spaceRegex = new(@"\s+", RegexOptions.Compiled);

    public AetheryteLinkInChatPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        linkPayload = pluginInterface.AddChatLinkHandler(LinkCommandId, HandleLink);
        solver = new AetheryteSolver(Dalamud.DataManager);
        teleporter = new Teleporter(Dalamud.Condition);
        Dalamud.ChatGui.ChatMessage += OnChatReceived;
        Dalamud.Condition.ConditionChange += OnConditionChanged;
    }

    public string MainCommandPrefix => "/alic";

    public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

    private void OnChatReceived(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        try
        {
            AppendNearestAetheryteLink(ref message);
        }
        catch (Exception exception)
        {
            PluginLog.Error(exception, nameof(OnChatReceived));
        }
    }

    private void OnConditionChanged(ConditionFlag flag, bool value)
    {
        if (!Config.AllowTeleportQueueing || teleporter.IsTeleportUnavailable)
        {
            return;
        }

        Task.Delay(Config.QueuedTeleportDelay)
            .ContinueWith(_ =>
            {
                var aetheryte = teleporter.TeleportToQueuedAetheryte();
                if (aetheryte != default)
                {
                    Divination.Chat.Print(Localization.QueuedTeleportingMessage.Format(aetheryte.PlaceName.Value?.Name.RawString));
                }
            });
    }

    private void AppendNearestAetheryteLink(ref SeString message)
    {
        var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
        if (mapLink == default)
        {
            PluginLog.Verbose("AppendNearestAetheryteLink: mapLink == null");
            return;
        }

        // 最短のテレポート経路を計算する
        var paths = solver.CalculateTeleportPathsForMapLink(mapLink).ToList();
        if (paths.Count == 0)
        {
            PluginLog.Debug("AppendNearestAetheryteLink: paths.Count == 0");
            return;
        }

        message = message.Append(new NewLinePayload());

        foreach (var (index, path) in paths.Select((x, i) => (i, x)))
        {
            switch (path)
            {
                // テレポ可能なエーテライト
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: true } aetheryte:
                    var payloads = new List<Payload>
                    {
                        new IconPayload(BitmapFontIcon.Aetheryte),
                        linkPayload,
                        new UIForegroundPayload(069),
                        new TextPayload(aetheryte.Aetheryte.PlaceName.Value?.Name.RawString),
                        UIForegroundPayload.UIForegroundOff,
                        RawPayload.LinkTerminator,
                    };
                    payloads.InsertRange(2, SeString.TextArrowPayloads);

                    message = message.Append(payloads);
                    break;
                // 仮設エーテライト・都市内エーテライト
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: false } aetheryte:
                    message = message.Append(new List<Payload>
                    {
                        new IconPayload(BitmapFontIcon.Aethernet),
                        new TextPayload(aetheryte.Aetheryte.AethernetName.Value?.Name.RawString),
                    });
                    break;
                // マップ境界
                case BoundaryTeleportPath boundary:
                    message = message.Append(new List<Payload>
                    {
                        new IconPayload(BitmapFontIcon.FlyZone),
                        new TextPayload(boundary.ConnectedMarker.PlaceNameSubtext.Value?.Name.RawString),
                    });
                    break;
            }

            if (index != paths.Count - 1)
            {
                message = message.Append(new TextPayload($" {SeIconChar.ArrowRight.ToIconString()} "));
            }
        }
    }

    private void HandleLink(uint id, SeString link)
    {
        PluginLog.Verbose("HandleLink: link = {Json}", link.ToJson());

        if (id != LinkCommandId)
        {
            PluginLog.Debug("HandleLink: id ({Id}) != LinkCommandId", id);
            return;
        }

        // 最初に矢印の TextPayload が入っているので除外する
        var aetheryteName = string.Join("", link.Payloads.OfType<TextPayload>().Skip(1).Select(x => x.Text));

        // 途中で改行された場合、正常にエーテライト名を取れないので空白文字を除去する
        // 今のところエーテライト名に空白が入るものは存在しないので問題ないと思う...
        // カスタムの RawPayload が実装できるようになったら実装を変更する
        aetheryteName = spaceRegex.Replace(aetheryteName, "");

        var aetheryte = solver.FindAetheryteByName(aetheryteName);
        if (aetheryte == default)
        {
            PluginLog.Error("HandleLink: aetheryte ({Name}) == null", aetheryteName);
            return;
        }

        if (teleporter.IsTeleportUnavailable)
        {
            teleporter.QueueTeleport(aetheryte);
            Divination.Chat.Print(Localization.QueueTeleportMessage.Format(aetheryte.PlaceName.Value?.Name.RawString));
        }
        else if (teleporter.TeleportToAetheryte(aetheryte))
        {
            Divination.Chat.Print(Localization.TeleportingMessage.Format(aetheryte.PlaceName.Value?.Name.RawString));
        }
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        Dalamud.PluginInterface.RemoveChatLinkHandler();
        Dalamud.ChatGui.ChatMessage -= OnChatReceived;
        Dalamud.Condition.ConditionChange -= OnConditionChanged;
    }
}
