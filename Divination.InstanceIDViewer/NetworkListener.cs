using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Game.Text;

namespace Divination.InstanceIDViewer
{
    public class NetworkListener : INetworkHandler
    {
        private readonly IChatClient chat;
        private readonly object lastServerIdLock = new();
        private ushort lastServerId;

        public NetworkListener(IChatClient chat)
        {
            this.chat = chat;
        }

        public bool CanHandleReceivedMessage(NetworkContext context) => true;

        public void HandleReceivedMessage(NetworkContext context)
        {
            var serverId = context.Header.ServerId;

            lock (lastServerIdLock)
            {
                if (serverId == default || serverId == lastServerId)
                {
                    return;
                }

                chat.Print(
                    $"[InstanceIDViewer] instance id changed: {lastServerId.ToString()} {SeIconChar.ArrowRight.ToIconString()} {serverId.ToString()}");

                if (serverId != 0)
                {
                    lastServerId = serverId;
                }
            }
        }
    }
}
