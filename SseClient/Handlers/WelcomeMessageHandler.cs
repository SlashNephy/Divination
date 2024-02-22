using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers;

public class WelcomeMessageHandler : ISsePayloadReceiver
{
    public bool CanReceive(string eventId) => eventId == "welcome";

    public void Receive(string eventId, SsePayload payload)
    {
        SseClient.Instance.Connection.IsDisconnected = false;
        SseClient.Instance.Connection.IsUnderMaintenance = false;

        SseClient.Instance.Divination.Chat.Print(payload.Message);
    }
}