namespace Divination.AetheryteLinkInChat.Solver;

/// <summary>
/// A spawn point of a hunt mark and the point (aetheryte or zone boundary) to start from, in raw map coordinates (0-2048).
/// </summary>
public record HuntJourney(int X, int Y, int JourneyX, int JourneyY);
