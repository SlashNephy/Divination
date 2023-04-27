using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers.BuiltIn
{
    public abstract class BuildInChatHandler : ISsePayloadReceiver, ISsePayloadEmitter
    {
        protected readonly XivChatType TargetChatType;
        protected readonly string EventId;

        public BuildInChatHandler(XivChatType targetChatType, string eventId)
        {
            TargetChatType = targetChatType;
            EventId = eventId;
        }

        public void Receive(string eventId, SsePayload payload)
        {
            SseUtils.PrintSseChat(new XivChatEntry
            {
                Type = SseClientPlugin.Instance.Config.MobHuntShoutMessagesType,
                Name = FormatShoutSenderName(payload),
                Message = payload.MessageSeString
            });
        }

        public void EmitChatMessage(XivChatType type, SeString sender, SeString message)
        {
            SseUtils.SendPayload(EventId, new SsePayload
            {
                ChatType = type,
                SenderSeString = sender,
                MessageSeString = message
            });
        }
    }
}
