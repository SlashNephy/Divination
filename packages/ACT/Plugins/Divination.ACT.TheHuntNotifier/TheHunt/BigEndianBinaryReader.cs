using System;
using System.IO;

namespace Divination.ACT.TheHuntNotifier.TheHunt
{
    internal class BigEndianBinaryReader : BinaryReader
    {
        public BigEndianBinaryReader(Stream stream) : base(stream)
        {
        }

        public override ushort ReadUInt16()
        {
            var data = ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public override uint ReadUInt32()
        {
            var data = ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }
    }
}
