using Dalamud.Game.Text;
using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers.MobHunt.Faloop
{
    public class FaloopSystemMessageHandler : ISsePayloadReceiver
    {
        public bool CanReceive(string eventId)
        {
            return eventId is "faloop_reconnected" or "faloop_disconnected" or "faloop_error" && SseClientPlugin.Instance.Config.ReceiveFaloopSystemMessages;
        }

        public void Receive(string eventId, SsePayload payload)
        {
            SseUtils.PrintSseChat(new XivChatEntry
            {
                Type = SseClientPlugin.Instance.Config.MobHuntFaloopSystemMessagesType,
                Name = "Faloop",
                Message = payload.Message
            });
        }
    }
}
