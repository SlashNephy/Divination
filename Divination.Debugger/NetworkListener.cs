using System;
using System.Collections.Concurrent;
using Dalamud.Divination.Common.Api.Network;

namespace Divination.Debugger
{
    public class NetworkListener : INetworkHandler, IDisposable
    {
        public static readonly BlockingCollection<NetworkContext> Contexts = new();

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

            Contexts.Add(context);
        }

        public void Dispose()
        {
            Contexts.Dispose();
        }
    }
}
