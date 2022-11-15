using System;

namespace Dalamud.Divination.Common.Api.Memory
{
    public static class ByteArrayEx
    {
        public static ushort[] ToUInt16Array(this byte[] source)
        {
            var array = new ushort[source.Length / 2];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToUInt16(source, i * 2);
            }

            return array;
        }

        public static short[] ToInt16Array(this byte[] source)
        {
            var array = new short[source.Length / 2];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToInt16(source, i * 2);
            }

            return array;
        }

        public static uint[] ToUInt32Array(this byte[] source)
        {
            var array = new uint[source.Length / 4];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToUInt32(source, i * 4);
            }

            return array;
        }

        public static int[] ToInt32Array(this byte[] source)
        {
            var array = new int[source.Length / 4];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToInt32(source, i * 4);
            }

            return array;
        }

        public static ulong[] ToUInt64Array(this byte[] source)
        {
            var array = new ulong[source.Length / 8];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToUInt32(source, i * 8);
            }

            return array;
        }

        public static long[] ToInt64Array(this byte[] source)
        {
            var array = new long[source.Length / 8];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = BitConverter.ToInt32(source, i * 8);
            }

            return array;
        }

        public static ushort ReadUInt16(this byte[] source, int offset)
        {
            return BitConverter.ToUInt16(source, offset);
        }

        public static short ReadInt16(this byte[] source, int offset)
        {
            return BitConverter.ToInt16(source, offset);
        }

        public static uint ReadUInt32(this byte[] source, int offset)
        {
            return BitConverter.ToUInt32(source, offset);
        }

        public static int ReadInt32(this byte[] source, int offset)
        {
            return BitConverter.ToInt32(source, offset);
        }

        public static ulong ReadUInt64(this byte[] source, int offset)
        {
            return BitConverter.ToUInt64(source, offset);
        }

        public static long ReadInt64(this byte[] source, int offset)
        {
            return BitConverter.ToInt64(source, offset);
        }
    }
}
