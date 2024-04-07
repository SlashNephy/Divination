using System;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using Divination.AetheryteLinkInChat.Solver;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat.Payloads;

public sealed class WorldTeleportPathPayload(WorldTeleportPath? path) : Payload
{
    public const byte Marker = 3;

    public WorldTeleportPath? Path => path;

    public override PayloadType Type => PayloadType.Unknown;

    protected override byte[] EncodeImpl()
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        return [
            Marker,
            ..MakePackedInteger(path.Aetheryte.RowId, path.World.RowId),
            ..MakePackedInteger(path.Marker.RowId, path.Map.RowId),
        ];
    }

    protected override void DecodeImpl(BinaryReader reader, long _)
    {
        Decode(reader);
    }

    public new void Decode(BinaryReader reader)
    {
        var (aetheryteId, worldId) = GetPackedIntegers(reader);
        var (markerId, mapId) = GetPackedIntegers(reader);

        path = new WorldTeleportPath(
            Aetheryte: DataResolver.GetExcelSheet<Aetheryte>()?.GetRow(aetheryteId) ?? throw new InvalidOperationException("invalid aetheryte ID"),
            World: DataResolver.GetExcelSheet<World>()?.GetRow(worldId) ?? throw new InvalidOperationException("invalid world ID"),
            Marker: DataResolver.GetExcelSheet<MapMarker>()?.GetRow(markerId) ?? throw new InvalidOperationException("invalid map marker ID"),
            Map: DataResolver.GetExcelSheet<Map>()?.GetRow(mapId) ?? throw new InvalidOperationException("invalid map ID")
        );
    }

    public override string ToString()
    {
        return $"{nameof(WorldTeleportPathPayload)}[{path}]";
    }
}
