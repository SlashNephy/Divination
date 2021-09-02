using System;
using System.Runtime.InteropServices;

namespace Dalamud.Divination.Common.Api.Network
{
    public static class NetworkContextEx
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
                structure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
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
}
