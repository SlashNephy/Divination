using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers;

public class GateAnnouncementsHandler : ISsePayloadReceiver, ISsePayloadEmitter
{
    private const XivChatType GoldSaucerChatType = (XivChatType) 68;

    private const string EventId = "gate_announcement";

    public bool CanReceive(string eventId)
    {
        return eventId == EventId && SseClient.Instance.Config.ReceiveGateAnnouncements;
    }

    public void Receive(string eventId, SsePayload payload)
    {
        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = payload.ChatType ?? GoldSaucerChatType,
            Name = SseUtils.FormatName(payload),
            Message = payload.MessageSeString
        });
    }

    public bool CanEmit(XivChatType chatType) => SseClient.Instance.Config.SendGateAnnouncements;

    public void EmitChatMessage(XivChatType type, SeString sender, SeString message)
    {
        if (sender.TextValue != "お客様案内窓口" && sender.TextValue != "運命の女神")
        {
            return;
        }

        SseUtils.SendPayload(EventId, new SsePayload
        {
            ChatType = type,
            SenderSeString = sender,
            MessageSeString = message
        });
    }
}