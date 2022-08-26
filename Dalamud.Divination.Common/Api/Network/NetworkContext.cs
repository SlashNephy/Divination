using System;
using System.Linq;
using Dalamud.Game.Network;

namespace Dalamud.Divination.Common.Api.Network
{
    public record NetworkContext
    {
        public DateTime Time { get; init; }
        public byte[] RawHeader { get; init; } = Array.Empty<byte>();
        public ushort Opcode { get; init; }
        public byte[] Data { get; init; } = Array.Empty<byte>();
        public IntPtr DataPtr { get; init; }
        public uint SourceActorId { get; init; }
        public uint TargetActorId { get; init; }
        public NetworkMessageDirection Direction { get; init; }

        public byte this[int i] => Data[i];
        public IpcHeader Header => RawHeader.ToStructure<IpcHeader>();

        public string ToString(int length)
        {
            var data = Data.Take(length).ToArray();

            var message = $"{(Direction == NetworkMessageDirection.ZoneUp ? "S" : "R")}{Time:HH:mm:ss.fff}";
            for (var i = 0; i < data.Length; i++)
            {
                message += $"({i:D2})";
            }

            message += $"{Environment.NewLine}    |0x{Opcode:X4}|  {BitConverter.ToString(data).Replace("-", "--")}";

            // byte
            message += $"{Environment.NewLine}    | {Opcode,5}|";
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (int val in data)
            {
                message += $" {val,3}";
            }

            // ushort
            message += $"{Environment.NewLine}            ";
            for (var i = 0; i < data.Length / 2; i++)
            {
                var val = BitConverter.ToUInt16(data, i * 2);
                message += $"  {val,6}";
            }

            // uint
            message += $"{Environment.NewLine}            ";
            for (var i = 0; i < data.Length / 4; i++)
            {
                var val = BitConverter.ToUInt32(data, i * 4);
                message += $"  {val,14}";
            }

            return message;
        }

        public override string ToString()
        {
            return ToString(128);
        }
    }
}
