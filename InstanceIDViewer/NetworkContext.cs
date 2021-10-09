using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Dalamud.Game.Network;

namespace InstanceIDViewer
{
    public class NetworkContext
    {
        public NetworkContext(byte[] header, ushort opcode, byte[] data, IntPtr dataPtr, uint sourceActorId,
            uint targetActorId, NetworkMessageDirection direction)
        {
            Time = DateTime.Now;
            RawHeader = header;
            Opcode = opcode;
            Data = data;
            DataPtr = dataPtr;
            SourceActorId = sourceActorId;
            TargetActorId = targetActorId;
            Direction = direction;
        }

        public DateTime Time { get; }
        public byte[] RawHeader { get; }
        public ushort Opcode { get; }
        public byte[] Data { get; }
        public IntPtr DataPtr { get; }
        public uint SourceActorId { get; }
        public uint TargetActorId { get; }
        public NetworkMessageDirection Direction { get; }

        public IPCHeader Header => RawHeader.ToStructure<IPCHeader>();

        public byte this[int i] => Data[i];

    }


    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal static class NetworkContextEx
    {
        public static ushort ReadUInt16(this NetworkContext context, int index)
        {
            return BitConverter.ToUInt16(context.Data, index);
        }

        public static uint ReadUInt32(this NetworkContext context, int index)
        {
            return BitConverter.ToUInt32(context.Data, index);
        }

        public static T ToStructure<T>(this NetworkContext context) where T : struct
        {
            return context.Data.ToStructure<T>();
        }

        public static T ToStructure<T>(this byte[] data) where T : struct
        {
            T structure;

            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                structure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }

            return structure;
        }

        public static bool TryToStructure<T>(this NetworkContext context, out T result) where T : struct
        {
            try
            {
                result = context.ToStructure<T>();
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    // ReSharper disable once InconsistentNaming
    public readonly struct IPCHeader
    {
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
    }
}
