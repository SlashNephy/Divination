using System;

namespace Divination.Debugger;

public static class ByteArrayUtils
{
    public static sbyte[] TransformIntoInt8(this byte[] source)
    {
        var array = new sbyte[source.Length];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = Convert.ToSByte(array[i]);
        }

        return array;
    }

    public static ushort[] TransformIntoUInt16(this byte[] source)
    {
        var array = new ushort[source.Length / 2];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = BitConverter.ToUInt16(source, i * 2);
        }

        return array;
    }

    public static short[] TransformIntoInt16(this byte[] source)
    {
        var array = new short[source.Length / 2];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = BitConverter.ToInt16(source, i * 2);
        }

        return array;
    }

    public static uint[] TransformIntoUInt32(this byte[] source)
    {
        var array = new uint[source.Length / 4];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = BitConverter.ToUInt32(source, i * 4);
        }

        return array;
    }

    public static int[] TransformIntoInt32(this byte[] source)
    {
        var array = new int[source.Length / 4];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = BitConverter.ToInt32(source, i * 4);
        }

        return array;
    }

    public static ulong[] TransformIntoUInt64(this byte[] source)
    {
        var array = new ulong[source.Length / 8];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = BitConverter.ToUInt64(source, i * 8);
        }

        return array;
    }

    public static long[] TransformIntoInt64(this byte[] source)
    {
        var array = new long[source.Length / 8];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = BitConverter.ToInt64(source, i * 8);
        }

        return array;
    }
}