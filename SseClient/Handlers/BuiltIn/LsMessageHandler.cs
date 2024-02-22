using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers.BuiltIn;

public class LsMessageHandler : ISsePayloadReceiver, ISsePayloadEmitter
{
    public string EventIdentifier => "linkshell_ls";

    // TODO: rename
    public bool CanReceive() => SseClient.Instance.Config.ReceiveMobHuntLsMessages;

    public void Receive(string eventId, SsePayload payload)
    {
        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntLsMessagesType,
            Name = SseUtils.FormatName(payload),
            Message = payload.MessageSeString
        });
    }

    public bool CanEmit(XivChatType chatType) => SseClient.Instance.Config.SendLs1Messages;

    public void EmitChatMessage(XivChatType type, SeString sender, SeString message)
    {
        this.EmitPayload(new SsePayload
        {
            ChatType = type,
            SenderSeString = sender,
            MessageSeString = message
        });
    }
}