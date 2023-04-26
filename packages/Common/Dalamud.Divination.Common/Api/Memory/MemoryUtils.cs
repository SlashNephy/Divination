using System;
using System.Runtime.InteropServices;

namespace Dalamud.Divination.Common.Api.Memory;

[Obsolete("Use Dalamud.Divination.Common.Api.Memory.IntPtrEx instead.")]
public static class MemoryUtils
{
    public static byte ReadByte(IntPtr address, int offset)
    {
        return Marshal.ReadByte(address, offset);
    }

    public static byte? ReadByte(IntPtr address, int? offset)
    {
        if (!offset.HasValue)
        {
            return null;
        }

        return Marshal.ReadByte(address, offset.Value);
    }

    public static bool WriteByte(IntPtr address, int? offset, byte? value)
    {
        if (!offset.HasValue || !value.HasValue)
        {
            return false;
        }

        Marshal.WriteByte(address, offset.Value, value.Value);
        return true;
    }

    public static short ReadInt16(IntPtr address, int offset)
    {
        return Marshal.ReadInt16(address, offset);
    }

    public static short? ReadInt16(IntPtr address, int? offset)
    {
        if (!offset.HasValue)
        {
            return null;
        }

        return Marshal.ReadInt16(address, offset.Value);
    }

    public static bool WriteInt16(IntPtr address, int? offset, short? value)
    {
        if (!offset.HasValue || !value.HasValue)
        {
            return false;
        }

        Marshal.WriteInt16(address, offset.Value, value.Value);
        return true;
    }

    public static int ReadInt32(IntPtr address, int offset)
    {
        return Marshal.ReadInt32(address, offset);
    }

    public static int? ReadInt32(IntPtr address, int? offset)
    {
        if (!offset.HasValue)
        {
            return null;
        }

        return Marshal.ReadInt32(address, offset.Value);
    }

    public static bool WriteInt32(IntPtr address, int? offset, int? value)
    {
        if (!offset.HasValue || !value.HasValue)
        {
            return false;
        }

        Marshal.WriteInt32(address, offset.Value, value.Value);
        return true;
    }

    public static long ReadInt64(IntPtr address, int offset)
    {
        return Marshal.ReadInt64(address, offset);
    }

    public static long? ReadInt64(IntPtr address, int? offset)
    {
        if (!offset.HasValue)
        {
            return null;
        }

        return Marshal.ReadInt64(address, offset.Value);
    }

    public static bool WriteInt64(IntPtr address, int? offset, long? value)
    {
        if (!offset.HasValue || !value.HasValue)
        {
            return false;
        }

        Marshal.WriteInt64(address, offset.Value, value.Value);
        return true;
    }
}
