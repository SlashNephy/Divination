using System;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using Divination.AetheryteLinkInChat.Solver;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat.Payloads;

public sealed class AetheryteTeleportPathPayload(AetheryteTeleportPath? path) : Payload
{
    public const byte Marker = 1;

    public AetheryteTeleportPath? Path => path;

    public override PayloadType Type => PayloadType.Unknown;

    protected override byte[] EncodeImpl()
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        return [
            Marker,
            ..MakeInteger(path.Aetheryte.RowId),
            ..MakePackedInteger(path.Marker.RowId, path.Map.RowId),
        ];
    }

    protected override void DecodeImpl(BinaryReader reader, long _)
    {
        Decode(reader);
    }

    public new void Decode(BinaryReader reader)
    {
        var aetheryteId = GetInteger(reader);
        var (markerId, mapId) = GetPackedIntegers(reader);

        path = new AetheryteTeleportPath(
            Aetheryte: DataResolver.GetExcelSheet<Aetheryte>()?.GetRow(aetheryteId) ?? throw new InvalidOperationException("invalid aetheryte ID"),
            Marker: DataResolver.GetExcelSheet<MapMarker>()?.GetRow(markerId) ?? throw new InvalidOperationException("invalid map marker ID"),
            Map: DataResolver.GetExcelSheet<Map>()?.GetRow(mapId) ?? throw new InvalidOperationException("invalid map ID")
        );
    }

    public override string ToString()
    {
        return $"{nameof(AetheryteTeleportPathPayload)}[{path}]";
    }
}
