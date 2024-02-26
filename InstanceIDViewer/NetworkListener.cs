using System;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Network;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text;

namespace Divination.InstanceIDViewer;

public class NetworkListener : INetworkHandler, IDisposable
{
    private readonly IChatClient chat;
    private readonly DtrBarEntry bar;

    private readonly object lastServerIdLock = new();
    private ushort lastServerId;

    public NetworkListener(IChatClient chat, DtrBarEntry bar)
    {
        this.chat = chat;
        this.bar = bar;
    }

    public bool CanHandleReceivedMessage(NetworkContext context)
    {
        return true;
    }

    public void HandleReceivedMessage(NetworkContext context)
    {
        var serverId = context.Header.ServerId;

        lock (lastServerIdLock)
        {
            if (serverId == default || serverId == lastServerId)
            {
                return;
            }

            chat.Print($"Instance ID changed: {lastServerId.ToString()} {SeIconChar.ArrowRight.ToIconString()} {serverId.ToString()}");
            bar.Text =
                $"{SeIconChar.ArrowDown.ToIconString()} {lastServerId.ToString()} {SeIconChar.ArrowRight.ToIconString()} {serverId.ToString()}";

            lastServerId = serverId;
        }
    }

    public void Dispose()
    {
        bar.Dispose();
    }
}
