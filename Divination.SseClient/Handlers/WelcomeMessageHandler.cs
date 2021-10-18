using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers
{
    public class WelcomeMessageHandler : ISsePayloadReceiver
    {
        public bool CanReceive(string eventId) => eventId == "welcome";

        public void Receive(string eventId, SsePayload payload)
        {
            SseClientPlugin.Instance.Connection.IsDisconnected = false;
            SseClientPlugin.Instance.Connection.IsUnderMaintenance = false;

            SseClientPlugin.Instance.Divination.Chat.Print(payload.Message);
        }
    }
}
