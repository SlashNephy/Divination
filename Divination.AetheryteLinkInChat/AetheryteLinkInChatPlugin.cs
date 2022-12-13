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
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class AetheryteLinkInChatPlugin : DivinationPlugin<AetheryteLinkInChatPlugin, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private const uint LinkCommandId = 0;
    private readonly DalamudLinkPayload linkPayload;
    private volatile uint queuedAetheryteId;

    public AetheryteLinkInChatPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        linkPayload = pluginInterface.AddChatLinkHandler(LinkCommandId, HandleLink);
        Dalamud.ChatGui.ChatMessage += OnChatReceived;
        Dalamud.Condition.ConditionChange += OnConditionChanged;
    }

    public string MainCommandPrefix => "/alic";

    public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

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
            PluginLog.Error(exception, nameof(OnChatReceived));
        }
    }

    private void OnConditionChanged(ConditionFlag flag, bool value)
    {
        if (queuedAetheryteId == default || !Config.AllowTeleportQueueing)
        {
            return;
        }

        switch (flag)
        {
            case ConditionFlag.InCombat when !value:
            case ConditionFlag.BetweenAreas when !value:
                Task.Delay(Config.QueuedTeleportDelay)
                    .ContinueWith(_ =>
                    {
                        TeleportToAetheryte(queuedAetheryteId);
                    });
                return;
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
            .Select(x => (
                aetheryte: x,
                marker: mapMarkers
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
        PluginLog.Verbose("HandleLink: link = {Json}", link.ToJson());

        if (id != LinkCommandId)
        {
            PluginLog.Debug("HandleLink: id ({Id}) != LinkCommandId", id);
            return;
        }

        // 最初には矢印の TextPayload が入っているので除外する
        var aetheryteName = string.Join("", link.Payloads.OfType<TextPayload>().Skip(1).Select(x => x.Text));

        // 途中で改行された場合、正常にエーテライト名を取れないので空白文字を除去する
        // 今のところエーテライト名に空白が入るものは存在しないので問題ないと思う...
        // カスタムの RawPayload が実装できるようになったら実装を変更する
        aetheryteName = new Regex(@"\s+").Replace(aetheryteName, "");

        var aetheryte = FindAetheryteByName(aetheryteName);
        if (aetheryte == default)
        {
            PluginLog.Error("HandleLink: aetheryte ({Name}) == null", aetheryteName);
            return;
        }

        if (Dalamud.Condition[ConditionFlag.InCombat] || Dalamud.Condition[ConditionFlag.BetweenAreas])
        {
            queuedAetheryteId = aetheryte.RowId;
            Divination.Chat.Print($"現在テレポを実行できません。「{aetheryteName}」へのテレポをキューに追加しました。");
        }
        else
        {
            TeleportToAetheryte(aetheryte.RowId);
        }
    }

    private Aetheryte? FindAetheryteByName(string name)
    {
        return Dalamud.DataManager.GetExcelSheet<Aetheryte>()
            ?.FirstOrDefault(x => x.PlaceName.Value?.Name.RawString == name);
    }

    private Aetheryte? FindAetheryteById(uint id)
    {
        return Dalamud.DataManager.GetExcelSheet<Aetheryte>()
            ?.FirstOrDefault(x => x.RowId == id);
    }

    private unsafe void TeleportToAetheryte(uint id)
    {
        queuedAetheryteId = default;

        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            PluginLog.Debug("TeleportToAetheryte: teleport == null");
            return;
        }

        if (!CheckAetheryte(teleport, id))
        {
            PluginLog.Error("TeleportToAetheryte: aetheryte with ID {Id} is invalid.", id);
        }
        else if (!teleport->Teleport(id, 0))
        {
            PluginLog.Error("TeleportToAetheryte: could not teleport to {Id}", id);
        }

        if (Config.PrintAetheryteName)
        {
            Divination.Chat.Print($"「{FindAetheryteById(id)?.PlaceName.Value?.Name.RawString}」にテレポしています...");
        }
    }

    private static unsafe bool CheckAetheryte(Telepo* teleport, uint id)
    {
        teleport->UpdateAetheryteList();

        for (var it = teleport->TeleportList.First; it != teleport->TeleportList.Last; it++)
        {
            if (it->AetheryteId == id)
            {
                return true;
            }
        }

        return false;
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        Dalamud.PluginInterface.RemoveChatLinkHandler();
        Dalamud.ChatGui.ChatMessage -= OnChatReceived;
        Dalamud.Condition.ConditionChange -= OnConditionChanged;
    }
}
