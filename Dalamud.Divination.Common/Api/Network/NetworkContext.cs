using System;
using System.Linq;
using Dalamud.Game.Network;

namespace Dalamud.Divination.Common.Api.Network
{
    public record NetworkContext
    {
        public DateTime Time { get; init; }
        public byte[] RawHeader { get; init; } = Array.Empty<byte>();
        public ushort Opcode { get; init; }
        public byte[] Data { get; init; } = Array.Empty<byte>();
        public IntPtr DataPtr { get; init; }
        public uint SourceActorId { get; init; }
        public uint TargetActorId { get; init; }
        public NetworkMessageDirection Direction { get; init; }

        public byte this[int i] => Data[i];
        public IpcHeader Header => RawHeader.ToStructure<IpcHeader>();
    }
}
