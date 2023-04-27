using System;
using System.Runtime.InteropServices;

namespace Dalamud.Divination.Common.Api.Memory;

public static class IntPtrEx
{
    public static byte[] Copy(this IntPtr address, int offset, int length)
    {
        var bytes = new byte[length];
        Marshal.Copy(address, bytes, offset, length);
        return bytes;
    }

    public static byte ReadByte(this IntPtr address, int offset)
    {
        return Marshal.ReadByte(address, offset);
    }

    public static void WriteByte(this IntPtr address, int offset, byte value)
    {
        Marshal.WriteByte(address, offset, value);
    }

    public static sbyte ReadSByte(this IntPtr address, int offset)
    {
        return Convert.ToSByte(address.ReadByte(offset));
    }

    public static void WriteSByte(this IntPtr address, int offset, sbyte value)
    {
        address.WriteByte(offset, Convert.ToByte(value));
    }

    public static short ReadInt16(this IntPtr address, int offset)
    {
        return Marshal.ReadInt16(address, offset);
    }

    public static void WriteInt16(this IntPtr address, int offset, short value)
    {
        Marshal.WriteInt16(address, offset, value);
    }

    public static ushort ReadUInt16(this IntPtr address, int offset)
    {
        return Convert.ToUInt16(address.ReadInt16(offset));
    }

    public static void WriteUInt16(this IntPtr address, int offset, ushort value)
    {
        address.WriteInt16(offset, Convert.ToInt16(value));
    }

    public static int ReadInt32(this IntPtr address, int offset)
    {
        return Marshal.ReadInt32(address, offset);
    }

    public static void WriteInt32(this IntPtr address, int offset, int value)
    {
        Marshal.WriteInt32(address, offset, value);
    }

    public static uint ReadUInt32(this IntPtr address, int offset)
    {
        return Convert.ToUInt32(address.ReadInt32(offset));
    }

    public static void WriteUInt32(this IntPtr address, int offset, uint value)
    {
        address.WriteInt32(offset, Convert.ToInt32(value));
    }

    public static long ReadInt64(this IntPtr address, int offset)
    {
        return Marshal.ReadInt64(address, offset);
    }

    public static void WriteInt64(this IntPtr address, int offset, long value)
    {
        Marshal.WriteInt64(address, offset, value);
    }

    public static ulong ReadUInt64(this IntPtr address, int offset)
    {
        return Convert.ToUInt64(address.ReadInt64(offset));
    }

    public static void WriteUInt64(this IntPtr address, int offset, ulong value)
    {
        address.WriteInt64(offset, Convert.ToInt64(value));
    }
}
