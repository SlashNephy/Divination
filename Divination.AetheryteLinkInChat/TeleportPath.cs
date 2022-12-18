using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public interface ITeleportPath
{
    public MapMarker Marker { get; }
    public Map Map { get; }
}

public record AetheryteTeleportPath(Aetheryte Aetheryte, MapMarker Marker, Map Map) : ITeleportPath
{
    public override string? ToString()
    {
        return Aetheryte.PlaceName.Value?.Name.RawString ?? Marker.PlaceNameSubtext.Value?.Name.RawString;
    }
}

public record BoundaryTeleportPath(MapMarker ConnectedMarker, Map ConnectedMap, MapMarker Marker, Map Map) : ITeleportPath
{
    public override string? ToString()
    {
        return ConnectedMarker.PlaceNameSubtext.Value?.Name.RawString;
    }
}
