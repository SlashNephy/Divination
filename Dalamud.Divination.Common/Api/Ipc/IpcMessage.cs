namespace Dalamud.Divination.Common.Api.Ipc
{
    public class IpcMessage
    {
        public readonly string Target;
        public readonly string Event;
        public readonly dynamic? Message;

        public IpcMessage(string target, string @event, dynamic? message)
        {
            Target = target;
            Event = @event;
            Message = message;
        }
    }
}
