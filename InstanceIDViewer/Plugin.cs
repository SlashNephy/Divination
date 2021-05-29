using System;
using System.Runtime.InteropServices;
using Dalamud.Game.Internal.Network;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;

namespace InstanceIDViewer
{
    public class InstanceIDViewer : IDalamudPlugin
    {
        private DalamudPluginInterface _pi;
        public string Name => "InstanceIDViewer";
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
                        new TextPayload("[InstanceIDViewer] "),
                        new TextPayload($"instance id changed: {_lastServerId} {(char) SeIconChar.ArrowRight} {serverId}"),
                    });

                    _pi?.Framework.Gui.Chat.PrintChat(new XivChatEntry
                    {
                      Type =   (XivChatType) 72, // Party Recruiting
                      Name = string.Empty,
                      MessageBytes = Message.Encode()
                    });
                }

                if (serverId != 0)
                {
                    _lastServerId = serverId;
                }
            }

        }
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            _pi = pluginInterface;
            _pi.Framework.Network.OnNetworkMessage += OnNetworkMessage;
        }

        public void Dispose()
        {
            _pi.Framework.Network.OnNetworkMessage -= OnNetworkMessage;
            _pi.Dispose();
        }
    }
}
