using System;
using Dalamud.Game.ClientState.Objects.Types;

namespace Dalamud.Divination.Common.Api.Memory;

[Obsolete("Use gameObject.Address.ReadXXX / WriteXXX directly.")]
public static class GameObjectEx
{
    public static byte ReadByte(this IGameObject gameObject, int offset)
    {
        return gameObject.Address.ReadByte(offset);
    }

    public static void WriteByte(this IGameObject gameObject, int offset, byte value)
    {
        gameObject.Address.WriteByte(offset, value);
    }

    public static short ReadInt16(this IGameObject gameObject, int offset)
    {
        return gameObject.Address.ReadInt16(offset);
    }

    public static void WriteInt16(this IGameObject gameObject, int offset, short value)
    {
        gameObject.Address.WriteInt16(offset, value);
    }

    public static int ReadInt32(this IGameObject gameObject, int offset)
    {
        return gameObject.Address.ReadInt32(offset);
    }

    public static void WriteInt32(this IGameObject gameObject, int offset, int value)
    {
        gameObject.Address.WriteInt32(offset, value);
    }

    public static long ReadInt64(this IGameObject gameObject, int offset)
    {
        return gameObject.Address.ReadInt64(offset);
    }

    public static void WriteInt64(this IGameObject gameObject, int offset, long value)
    {
        gameObject.Address.WriteInt64(offset, value);
    }
}
