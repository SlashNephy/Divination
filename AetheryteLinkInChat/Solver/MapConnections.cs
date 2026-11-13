using System.Collections.Generic;

namespace Divination.AetheryteLinkInChat.Solver;

/// <summary>
/// A connection from a point on a map to another map, which the game's map markers do not describe.
/// </summary>
/// <param name="MapId">The map that has the exit.</param>
/// <param name="X">The raw X coordinate (0-2048) of the map marker of the exit.</param>
/// <param name="Y">The raw Y coordinate (0-2048) of the map marker of the exit.</param>
/// <param name="ConnectedMapId">The map that the exit leads to.</param>
public record MapConnection(uint MapId, int X, int Y, uint ConnectedMapId);

public static class MapConnections
{
    public static readonly IReadOnlyList<MapConnection> Connections =
    [
        // Yanxia <-> The Doman Enclave (The One River). The marker in Yanxia points to the obsolete map 437, and the current
        // map 463 has no marker back to Yanxia.
        new(354, 530, 1567, 463),
    ];
}
