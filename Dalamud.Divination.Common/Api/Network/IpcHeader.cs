using System.Runtime.InteropServices;

namespace Dalamud.Divination.Common.Api.Network
{
    [StructLayout(LayoutKind.Explicit, Size = IpcHeaderLength)]
    public readonly struct IpcHeader
    {
        public const int IpcHeaderLength = 32;

        [FieldOffset(0x00)] public readonly ushort Reserved1;
        [FieldOffset(0x02)] public readonly ushort Reserved2;
        [FieldOffset(0x04)] public readonly ushort Reserved3;
        [FieldOffset(0x06)] public readonly ushort Reserved4;
        [FieldOffset(0x08)] public readonly ushort Reserved5;
        [FieldOffset(0x0A)] public readonly ushort Reserved6;
        [FieldOffset(0x0C)] public readonly ushort Reserved7;
        [FieldOffset(0x0E)] public readonly ushort Reserved8;
        [FieldOffset(0x10)] public readonly ushort Reserved9;
        [FieldOffset(0x12)] public readonly ushort Opcode;
        [FieldOffset(0x14)] public readonly ushort Padding1;
        [FieldOffset(0x16)] public readonly ushort ServerId;
        [FieldOffset(0x18)] public readonly uint TimeStamp;
        [FieldOffset(0x1A)] public readonly uint Padding2;

        public override string ToString()
        {
            return
                $"IpcHeader({nameof(Opcode)} = 0x{Opcode:X4}, {nameof(ServerId)} = {ServerId}, {nameof(TimeStamp)} = {TimeStamp})";
        }
    }
}
