using System;
using System.Runtime.InteropServices;
using Dalamud.Game.Network;

namespace Dalamud.Divination.Common.Api.Network;

internal sealed class NetworkDataParser : IDisposable
{
    public delegate void OnNetworkContextDelegate(NetworkContext context);
    private readonly GameNetwork gameNetwork;
    private readonly int maxByteLength;
    public OnNetworkContextDelegate? OnNetworkContext;

    public NetworkDataParser(GameNetwork gameNetwork, int maxByteLength = 1024)
    {
        this.gameNetwork = gameNetwork;
        this.gameNetwork.NetworkMessage += OnNetworkMessage;
        this.maxByteLength = maxByteLength;
    }

    public void Dispose()
    {
        gameNetwork.NetworkMessage -= OnNetworkMessage;
    }

    private void OnNetworkMessage(IntPtr dataPtr,
        ushort opcode,
        uint sourceActorId,
        uint targetActorId,
        NetworkMessageDirection direction)
    {
        if (OnNetworkContext == null)
        {
            return;
        }

        var header = new byte[IpcHeader.IpcHeaderLength];
        if (direction == NetworkMessageDirection.ZoneDown)
        {
            Marshal.Copy(dataPtr - 0x20, header, 0, header.Length);
        }

        var data = new byte[maxByteLength];
        Marshal.Copy(dataPtr, data, 0, data.Length);

        var context = new NetworkContext
        {
            Time = DateTime.Now,
            RawHeader = header,
            Opcode = opcode,
            Data = data,
            DataPtr = dataPtr,
            SourceActorId = sourceActorId,
            TargetActorId = targetActorId,
            Direction = direction,
        };
        OnNetworkContext.Invoke(context);
    }
}
