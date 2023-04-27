using Divination.SseClient.Payloads;

namespace Divination.SseClient.Handlers
{
    public interface ISsePayloadReceiver : ISsePayloadHandler
    {
        public bool CanReceive(string eventId);
        public void Receive(string eventId, SsePayload payload);
    }
}
