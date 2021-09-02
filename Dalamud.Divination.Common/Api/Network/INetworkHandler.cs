namespace Dalamud.Divination.Common.Api.Network
{
    public interface INetworkHandler
    {
        bool CanHandleReceivedMessage(NetworkContext context);

        void HandleReceivedMessage(NetworkContext context);

        bool CanHandleSentMessage(NetworkContext context);

        void HandleSentMessage(NetworkContext context);
    }
}
