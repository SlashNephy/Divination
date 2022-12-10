using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class AetheryteLinkInChatPlugin : DivinationPlugin<AetheryteLinkInChatPlugin>, IDalamudPlugin
{
    private const uint LinkCommandId = 0;
    private readonly DalamudLinkPayload linkPayload;

    public AetheryteLinkInChatPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        linkPayload = pluginInterface.AddChatLinkHandler(LinkCommandId, HandleLink);
        Dalamud.ChatGui.ChatMessage += OnChatReceived;
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.RemoveChatLinkHandler();
        Dalamud.ChatGui.ChatMessage -= OnChatReceived;
    }

    private void OnChatReceived(XivChatType type,
        uint senderId,
        ref SeString sender,
        ref SeString message,
        ref bool isHandled)
    {
        try
        {
            AppendNearestAetheryteLink(ref message);
        }
        catch (Exception exception)
        {
            PluginLog.Error(exception, "OnChatReceived");
        }
    }

    private void AppendNearestAetheryteLink(ref SeString message)
    {
        var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
        if (mapLink == default)
        {
            PluginLog.Verbose("AppendNearestAetheryteLink: mapLink == null");
            return;
        }

        var aetherytes = Dalamud.DataManager.GetExcelSheet<Aetheryte>();
        if (aetherytes == default)
        {
            PluginLog.Debug("AppendNearestAetheryteLink: aetherytes == null");
            return;
        }

        var mapMarkers = Dalamud.DataManager.GetExcelSheet<MapMarker>();
        if (mapMarkers == default)
        {
            PluginLog.Debug("AppendNearestAetheryteLink: mapMarkers == null");
            return;
        }

        // 対象のエリア内の最も近いエーテライトを探す
        var nearestAetheryte = aetherytes
            // 対象のエリア内に限定
            .Where(x => x.Territory.Row == mapLink.TerritoryType.RowId)
            // MapMarker に変換
            .Select(x => (aetheryte: x, marker: mapMarkers
                // エーテライトのマーカーに限定
                .Where(marker => marker.DataType is 3 or 4)
                .FirstOrDefault(marker => marker.DataKey == x.RowId)))
            .Where(x => x.marker != null)
            // 座標を変換
            .OrderBy(x =>
                Math.Pow(x.marker!.X * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1 - mapLink.XCoord, 2) +
                Math.Pow(x.marker.Y * 42.0 / 2048 / mapLink.Map.SizeFactor * 100 + 1 - mapLink.YCoord, 2))
            .FirstOrDefault();
        if (nearestAetheryte == default)
        {
            PluginLog.Debug("AppendNearestAetheryteLink: nearestAetheryte == null");
            return;
        }

        // テレポ可能なエーテライト
        if (nearestAetheryte.aetheryte.IsAetheryte)
        {
            var payloads = new List<Payload>
            {
                new IconPayload(BitmapFontIcon.Aetheryte),
                linkPayload,
                new TextPayload(nearestAetheryte.aetheryte.PlaceName.Value?.Name.RawString),
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
                new TextPayload(nearestAetheryte.aetheryte.AethernetName.Value?.Name.RawString),
            });
        }
    }

    private void HandleLink(uint id, SeString link)
    {
        PluginLog.Verbose("Link = {Json}", link.ToJson());

        if (id != LinkCommandId)
        {
            PluginLog.Debug("HandleLink: id ({Id}) != LinkCommandId", id);
            return;
        }

        // 途中で改行された場合、正常にエーテライト名を取れないので空白文字を除去する
        // 今のところエーテライト名に空白が入ることはないので問題ないと思う...
        // カスタムの RawPayload が実装できるようになったら実装を変更する
        var aetheryteName = string.Join("", link.Payloads.OfType<TextPayload>().Skip(1).Select(x => x.Text));
        aetheryteName = new Regex(@"\s+").Replace(aetheryteName, "");

        var aetheryte = FindAetheryteByName(aetheryteName);
        if (aetheryte == default)
        {
            PluginLog.Error("HandleLink: aetheryte ({Name}) == null", aetheryteName);
            return;
        }

        TeleportToAetheryte(aetheryte);
    }

    private Aetheryte? FindAetheryteByName(string name)
    {
        var aetherytes = Dalamud.DataManager.GetExcelSheet<Aetheryte>();
        return aetherytes?.FirstOrDefault(x => x.PlaceName.Value?.Name.RawString == name);
    }

    private unsafe void TeleportToAetheryte(Aetheryte aetheryte)
    {
        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            PluginLog.Debug("TeleportToAetheryte: teleport == null");
            return;
        }

        teleport->UpdateAetheryteList();

        var found = false;
        for (var it = teleport->TeleportList.First; it != teleport->TeleportList.Last; ++it)
        {
            if (it->AetheryteId == aetheryte.RowId)
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            PluginLog.Error("Aetheryte with ID {Id} is invalid.", aetheryte.RowId);
        }
        else if (!teleport->Teleport(aetheryte.RowId, 0))
        {
            PluginLog.Error($"Could not teleport to {Name}", aetheryte.PlaceName.Value?.Name ?? string.Empty);
        }
    }
}
