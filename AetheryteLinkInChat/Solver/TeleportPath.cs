using Lumina.Excel.Sheets;

namespace Divination.AetheryteLinkInChat.Solver;

public interface ITeleportPath
{
    public MapMarker Marker { get; }
    public Map Map { get; }
}

public record AetheryteTeleportPath(Aetheryte Aetheryte, MapMarker Marker, Map Map) : ITeleportPath
{
    public override string ToString()
    {
        return Aetheryte.PlaceName.IsValid ? Aetheryte.PlaceName.Value.Name.ExtractText() : Marker.PlaceNameSubtext.Value.Name.ExtractText();
    }
}

public record BoundaryTeleportPath(MapMarker ConnectedMarker, Map ConnectedMap, MapMarker Marker, Map Map) : ITeleportPath
{
    public override string ToString()
    {
        return ConnectedMarker.PlaceNameSubtext.Value.Name.ExtractText();
    }
}

public record WorldTeleportPath(Aetheryte Aetheryte, World World, MapMarker Marker, Map Map) : ITeleportPath
{
    public override string ToString()
    {
        return World.Name.ExtractText();
    }
}
