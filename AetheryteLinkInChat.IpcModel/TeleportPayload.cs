using System.Numerics;

namespace Divination.AetheryteLinkInChat.IpcModel;

public record struct TeleportPayload
{
    public const string Name = "Divination.AetheryteLinkInChat_Teleport";

    public uint TerritoryTypeId { get; init; }
    public uint MapId { get; init; }
    public Vector2 Coordinates { get; init; }
    public uint? WorldId { get; init; }
}
