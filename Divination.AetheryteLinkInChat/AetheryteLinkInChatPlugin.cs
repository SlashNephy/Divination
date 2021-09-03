using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat
{
    public class AetheryteLinkInChatPlugin : DivinationPlugin<AetheryteLinkInChatPlugin, PluginConfig>,
        IConfigWindowSupport<PluginConfig>
    {
        private const int LinkCommandId = 1;
        private readonly DalamudLinkPayload? linkPayload;

        public AetheryteLinkInChatPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            linkPayload = pluginInterface.AddChatLinkHandler(LinkCommandId, HandleAetheryteLink);
            Dalamud.ChatGui.ChatMessage += OnChatReceived;
        }

        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

        private void OnChatReceived(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            try
            {
                if (Config.Enabled)
                {
                    AppendNearestAetheryteLink(ref message);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while OnChatReceived");
            }
        }

        private void AppendNearestAetheryteLink(ref SeString message)
        {
            var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
            if (mapLink == null)
            {
                return;
            }

            // 対象のエリア内の最も近いエーテライトを探す
            var nearestAetheryte = Dalamud.DataManager.GetExcelSheet<Aetheryte>()!
                // 対象のエリア内に限定
                .Where(x => x.Territory.Row == mapLink.TerritoryType.RowId)
                // MapMarker に変換
                .Select(x => (
                    aetheryte: x,
                    marker: Dalamud.DataManager.GetExcelSheet<MapMarker>()!
                        // エーテライトのマーカーに限定
                        .Where(marker => marker.DataType is 3 or 4)
                        .FirstOrDefault(marker => marker.DataKey == x.RowId)
                ))
                .Where(x => x.marker != null)
                // 座標を変換
                .OrderBy(x => Math.Pow((x.marker!.X * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1) - mapLink.XCoord, 2) + Math.Pow((x.marker.Y * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1) - mapLink.YCoord, 2))
                .FirstOrDefault();
            if (nearestAetheryte == default)
            {
                return;
            }

            // テレポ可能なエーテライト
            if (nearestAetheryte.aetheryte.IsAetheryte)
            {
                var extension = new List<Payload>
                {
                    new IconPayload(BitmapFontIcon.Aetheryte),
                    linkPayload!,
                    new TextPayload($"{nearestAetheryte.aetheryte.PlaceName.Value!.Name.RawString}"),
                    RawPayload.LinkTerminator
                };
                extension.InsertRange(2, SeString.TextArrowPayloads());
                message = message.Append(extension);
            }
            // 仮設エーテライト・都市内エーテライト
            else
            {
                message = message.Append(new List<Payload>
                {
                    new IconPayload(BitmapFontIcon.Aethernet),
                    new TextPayload($"{nearestAetheryte.aetheryte.AethernetName.Value!.Name.RawString}")
                });
            }
        }

        private void HandleAetheryteLink(uint id, SeString link)
        {
            if (id != LinkCommandId)
            {
                return;
            }

            Dalamud.CommandManager.ProcessCommand($"/tp {link.Payloads.OfType<TextPayload>().Last().Text}");
        }

        protected override void ReleaseManaged()
        {
            Dalamud.ChatGui.ChatMessage -= OnChatReceived;
            Dalamud.PluginInterface.RemoveChatLinkHandler();
        }
    }
}
