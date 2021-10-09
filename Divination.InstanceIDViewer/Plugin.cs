using System;
using System.Runtime.InteropServices;
using Dalamud.Game.Gui;
using Dalamud.Game.Network;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Divination.InstanceIDViewer
{
    public class InstanceIDViewer : IDalamudPlugin
    {

        [PluginService]
        public static DalamudPluginInterface Interface { get; private set; }

        [PluginService]
        public static ChatGui ChatGui { get; private set; }

        [PluginService]
        public static GameNetwork GameNetwork { get; private set; }
        
        public string Name => "Divination.InstanceIDViewer";
        private static readonly object LastServerIdLock = new object();
        private ushort _lastServerId;

        private void OnNetworkMessage(IntPtr dataPtr, ushort opcode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
        {
            var header = new byte[32];
            if (direction == NetworkMessageDirection.ZoneDown)
            {
                Marshal.Copy(dataPtr - 0x20, header, 0, header.Length);
            }

            var data = new byte[32];
            Marshal.Copy(dataPtr, data, 0, data.Length);

            var context = new NetworkContext(header, opcode, data, dataPtr, sourceActorId, targetActorId, direction);

            var serverId = context.Header.ServerId;

            lock (LastServerIdLock)
            {
                if (serverId != default && serverId != _lastServerId)
                {

                    var Message = new SeString(new Payload[]
                    {
                        new TextPayload("[Divination.InstanceIDViewer] "),
                        new TextPayload($"instance id changed: {_lastServerId} {(char) SeIconChar.ArrowRight} {serverId}"),
                    });
                    ChatGui.PrintChat(new XivChatEntry
                    {
                      Type =   (XivChatType) 72, // Party Recruiting
                      Name = string.Empty,
                      Message = Message
                    });
                }

                if (serverId != 0)
                {
                    _lastServerId = serverId;
                }
            }
        }
        public InstanceIDViewer()
        {
            GameNetwork.NetworkMessage += OnNetworkMessage;
        }

        public void Dispose()
        {
            GameNetwork.NetworkMessage -= OnNetworkMessage;
        }
    }
}
