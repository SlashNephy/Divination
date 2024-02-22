using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers;

public class MaintenanceHandler : ISsePayloadReceiver
{
    public bool CanReceive(string eventId) => eventId == "maintenance";

    public void Receive(string eventId, SsePayload payload)
    {
        SseClient.Instance.Connection.IsUnderMaintenance = true;
        SseClient.Instance.Divination.Chat.PrintError(payload.Message);
    }
}