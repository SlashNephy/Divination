namespace Dalamud.Divination.Common.Api.Network;

public interface INetworkHandler
{
    bool CanHandleReceivedMessage(NetworkContext context)
    {
        return false;
    }

    void HandleReceivedMessage(NetworkContext context)
    {
    }

    bool CanHandleSentMessage(NetworkContext context)
    {
        return false;
    }

    void HandleSentMessage(NetworkContext context)
    {
    }
}
