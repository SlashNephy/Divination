using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers
{
    public class MaintenanceHandler : ISsePayloadReceiver
    {
        public bool CanReceive(string eventId) => eventId == "maintenance";

        public void Receive(string eventId, SsePayload payload)
        {
            SseClientPlugin.Instance.Connection.IsUnderMaintenance = true;
            SseClientPlugin.Instance.Divination.Chat.PrintError(payload.Message);
        }
    }
}
