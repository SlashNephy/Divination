using Dalamud.Game.ClientState.Objects.Types;

namespace Dalamud.Divination.Common.Api.Memory
{
    public static class GameObjectEx
    {
        public static byte ReadByte(this GameObject gameObject, int offset)
        {
            return MemoryUtils.ReadByte(gameObject.Address, offset);
        }

        public static byte? ReadByte(this GameObject gameObject, int? offset)
        {
            return MemoryUtils.ReadByte(gameObject.Address, offset);
        }

        public static bool WriteByte(this GameObject gameObject, int? offset, byte? value)
        {
            return MemoryUtils.WriteByte(gameObject.Address, offset, value);
        }

        public static short ReadInt16(this GameObject gameObject, int offset)
        {
            return MemoryUtils.ReadInt16(gameObject.Address, offset);
        }

        public static short? ReadInt16(this GameObject gameObject, int? offset)
        {
            return MemoryUtils.ReadInt16(gameObject.Address, offset);
        }

        public static bool WriteInt16(this GameObject gameObject, int? offset, short? value)
        {
            return MemoryUtils.WriteInt16(gameObject.Address, offset, value);
        }

        public static int ReadInt32(this GameObject gameObject, int offset)
        {
            return MemoryUtils.ReadInt32(gameObject.Address, offset);
        }

        public static int? ReadInt32(this GameObject gameObject, int? offset)
        {
            return MemoryUtils.ReadInt32(gameObject.Address, offset);
        }

        public static bool WriteInt32(this GameObject gameObject, int? offset, int? value)
        {
            return MemoryUtils.WriteInt32(gameObject.Address, offset, value);
        }

        public static long ReadInt64(this GameObject gameObject, int offset)
        {
            return MemoryUtils.ReadInt64(gameObject.Address, offset);
        }

        public static long? ReadInt64(this GameObject gameObject, int? offset)
        {
            return MemoryUtils.ReadInt64(gameObject.Address, offset);
        }

        public static bool WriteInt64(this GameObject gameObject, int? offset, long? value)
        {
            return MemoryUtils.WriteInt64(gameObject.Address, offset, value);
        }
    }
}
