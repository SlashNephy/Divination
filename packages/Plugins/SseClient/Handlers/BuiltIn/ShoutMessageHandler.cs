using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.SseClient.Payloads;
using ImGuiNET;

namespace Divination.SseClient.Handlers.BuiltIn;

public class ShoutMessageHandler : ISsePayloadReceiver, ISsePayloadEmitter
{
    public string EventIdentifier => "shout";

    public bool CanEmit(XivChatType chatType) => SseClient.Instance.Config.SendShoutMessages;

    public void EmitChatMessage(XivChatType type, SeString sender, SeString message)
    {
        this.EmitPayload(new SsePayload
        {
            ChatType = type,
            SenderSeString = sender,
            MessageSeString = message
        });
    }

    public bool CanReceive() => SseClient.Instance.Config.ReceiveMobHuntShoutMessages;

    private readonly Dictionary<string, bool> test = new Dictionary<string, bool>();

    public void Receive(string eventId, SsePayload payload)
    {
        SseUtils.PrintSseChat(new XivChatEntry
        {
            Type = SseClient.Instance.Config.MobHuntShoutMessagesType,
            Name = FormatShoutSenderName(payload),
            Message = payload.MessageSeString
        });
    }
}