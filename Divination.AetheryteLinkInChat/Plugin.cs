using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat
{
    public class Plugin : DivinationPlugin<Plugin>, IDalamudPlugin
    {
        private const int LinkCommandId = 1;
        private readonly DalamudLinkPayload linkPayload;

        public Plugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            linkPayload = pluginInterface.AddChatLinkHandler(LinkCommandId, HandleAetheryteLink);
            Dalamud.ChatGui.ChatMessage += OnChatReceived;
        }

        protected override void ReleaseManaged()
        {
            Dalamud.PluginInterface.RemoveChatLinkHandler();
            Dalamud.ChatGui.ChatMessage -= OnChatReceived;
        }

        private void OnChatReceived(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            try
            {
                AppendNearestAetheryteLink(ref message);
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Error occurred in OnChatReceived");
            }
        }

        private void AppendNearestAetheryteLink(ref SeString message)
        {
            var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
            if (mapLink == null)
            {
                return;
            }

            var aetherytes = Dalamud.DataManager.GetExcelSheet<Aetheryte>();
            if (aetherytes == default)
            {
                return;
            }

            var mapMarkers = Dalamud.DataManager.GetExcelSheet<MapMarker>();
            if (mapMarkers == default)
            {
                return;
            }

            // 対象のエリア内の最も近いエーテライトを探す
            var nearestAetheryte = aetherytes
                // 対象のエリア内に限定
                .Where(x => x.Territory.Row == mapLink.TerritoryType.RowId)
                // MapMarker に変換
                .Select(x => (
                    aetheryte: x,
                    marker: mapMarkers
                        // エーテライトのマーカーに限定
                        .Where(marker => marker.DataType is 3 or 4)
                        .FirstOrDefault(marker => marker.DataKey == x.RowId)
                ))
                .Where(x => x.marker != null)
                // 座標を変換
                .OrderBy(x =>
                    Math.Pow((x.marker!.X * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1) - mapLink.XCoord, 2) +
                    Math.Pow((x.marker.Y * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1) - mapLink.YCoord, 2))
                .FirstOrDefault();
            if (nearestAetheryte == default)
            {
                return;
            }

            // テレポ可能なエーテライト
            if (nearestAetheryte.aetheryte.IsAetheryte)
            {
                var payloads = new List<Payload>
                {
                    new IconPayload(BitmapFontIcon.Aetheryte),
                    linkPayload,
                    new TextPayload($"{nearestAetheryte.aetheryte.PlaceName.Value?.Name.RawString}"),
                    RawPayload.LinkTerminator,
                };
                payloads.InsertRange(2, SeString.TextArrowPayloads);
                message = message.Append(payloads);
            }
            // 仮設エーテライト・都市内エーテライト
            else
            {
                message = message.Append(new List<Payload>
                {
                    new IconPayload(BitmapFontIcon.Aethernet),
                    new TextPayload($"{nearestAetheryte.aetheryte.AethernetName.Value?.Name.RawString}"),
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
    }
}
