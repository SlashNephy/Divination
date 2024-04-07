using System;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using Divination.AetheryteLinkInChat.Solver;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat.Payloads;

public sealed class BoundaryTeleportPathPayload(BoundaryTeleportPath? path) : Payload
{
    public const byte Marker = 2;

    public BoundaryTeleportPath? Path => path;

    public override PayloadType Type => PayloadType.Unknown;

    protected override byte[] EncodeImpl()
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        return [
            Marker,
            ..MakePackedInteger(path.ConnectedMarker.RowId, path.ConnectedMap.RowId),
            ..MakePackedInteger(path.Marker.RowId, path.Map.RowId),
        ];
    }

    protected override void DecodeImpl(BinaryReader reader, long _)
    {
        Decode(reader);
    }

    public new void Decode(BinaryReader reader)
    {
        var (connectedMarkerId, connectedMapId) = GetPackedIntegers(reader);
        var (markerId, mapId) = GetPackedIntegers(reader);

        path = new BoundaryTeleportPath(
            ConnectedMarker: DataResolver.GetExcelSheet<MapMarker>()?.GetRow(connectedMarkerId) ?? throw new InvalidOperationException("invalid connected map marker ID"),
            ConnectedMap: DataResolver.GetExcelSheet<Map>()?.GetRow(connectedMapId) ?? throw new InvalidOperationException("invalid connected map ID"),
            Marker: DataResolver.GetExcelSheet<MapMarker>()?.GetRow(markerId) ?? throw new InvalidOperationException("invalid map marker ID"),
            Map: DataResolver.GetExcelSheet<Map>()?.GetRow(mapId) ?? throw new InvalidOperationException("invalid map ID")
        );
    }

    public override string ToString()
    {
        return $"{nameof(BoundaryTeleportPathPayload)}[{path}]";
    }
}
