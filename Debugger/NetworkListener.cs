using System;
using System.Collections.Concurrent;
using Dalamud.Divination.Common.Api.Network;

namespace Divination.Debugger;

public class NetworkListener : INetworkHandler, IDisposable
{
    public static readonly BlockingCollection<NetworkContext> Contexts = new();

    public bool CanHandleReceivedMessage(NetworkContext context)
    {
        return Debugger.Instance.Config.NetworkEnableListener && Debugger.Instance.Config.NetworkListenDownload;
    }

    public void HandleReceivedMessage(NetworkContext context)
    {
        Feed(context);
    }

    public bool CanHandleSentMessage(NetworkContext context)
    {
        return Debugger.Instance.Config.NetworkEnableListener && Debugger.Instance.Config.NetworkListenUpload;
    }

    public void HandleSentMessage(NetworkContext context)
    {
        Feed(context);
    }

    private static void Feed(NetworkContext context)
    {
        if (Debugger.Instance.Config.NetworkEnableOpcodeFilter && context.Opcode != Debugger.Instance.Config.NetworkFilterOpcode)
        {
            return;
        }

        Contexts.Add(context);
    }

    public void Dispose()
    {
        Contexts.Dispose();
    }
}
