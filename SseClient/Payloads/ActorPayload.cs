using System;

#pragma warning disable 8618

namespace Divination.SseClient.Payloads;

public class ActorPayload
{
    public DateTime Time { get; set; }
    public int NameId { get; set; }
    public uint MonsterId { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public TimeSpan? TimeToDeath { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public ushort TerritoryType { get; set; }
    public uint WorldId { get; set; }
}