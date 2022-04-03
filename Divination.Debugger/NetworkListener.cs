using Dalamud.Divination.Common.Api.Network;

namespace Divination.Debugger;

public class NetworkListener : INetworkHandler
{
    private static readonly object LastContextLock = new();
    private static NetworkContext? _lastContext;

    public static NetworkContext? LastContext
    {
        get
        {
            lock (LastContextLock)
            {
                return _lastContext;
            }
        }
    }

    public bool CanHandleReceivedMessage(NetworkContext context) => DebuggerPlugin.Instance.Config.NetworkEnableListener && DebuggerPlugin.Instance.Config.NetworkListenDownload;

    public void HandleReceivedMessage(NetworkContext context) => Feed(context);

    public bool CanHandleSentMessage(NetworkContext context) => DebuggerPlugin.Instance.Config.NetworkEnableListener && DebuggerPlugin.Instance.Config.NetworkListenUpload;

    public void HandleSentMessage(NetworkContext context) => Feed(context);

    private static void Feed(NetworkContext context)
    {
        if (DebuggerPlugin.Instance.Config.NetworkEnableOpcodeFilter && context.Opcode != DebuggerPlugin.Instance.Config.NetworkFilterOpcode)
        {
            return;
        }

        lock (LastContextLock)
        {
            _lastContext = context;
        }
    }
}
