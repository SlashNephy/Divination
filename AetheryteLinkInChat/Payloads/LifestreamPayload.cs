using System.Collections.Generic;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat.Payloads;

public sealed class LifestreamPayload(MapLinkPayload mapLink, uint? worldId) : DalamudLinkPayload
{
    private const byte EmbeddedInfoTypeByte = AetherytePayload.EmbeddedInfoTypeByte + 1;

    public MapLinkPayload MapLink => mapLink;
    public World? World => worldId.HasValue ? AetheryteLinkInChat.Instance.Dalamud.DataManager.GetExcelSheet<World>()?.GetRow(worldId.Value) : default;

    public override PayloadType Type => PayloadType.Unknown;

    private LifestreamPayload() : this(new MapLinkPayload(0, 0, 0, 0), 0)
    {
    }

    protected override byte[] EncodeImpl()
    {
        var data = new List<byte>();
        data.AddRange(mapLink.Encode());
        data.AddRange(MakeInteger(worldId ?? 0));

        var length = 2 + (byte)data.Count;
        return [
            START_BYTE,
            (byte)SeStringChunkType.Interactable,
            (byte)length,
            EmbeddedInfoTypeByte,
            .. data,
            END_BYTE,
        ];
    }

    protected override void DecodeImpl(BinaryReader reader, long _)
    {
        mapLink = (MapLinkPayload)Decode(reader);
        worldId = GetInteger(reader);
    }

    public override string ToString()
    {
        return $"{nameof(LifestreamPayload)}[{mapLink}, {worldId}]";
    }

    public RawPayload ToRawPayload()
    {
        return new RawPayload(EncodeImpl());
    }

    public static LifestreamPayload? Parse(RawPayload payload)
    {
        using var stream = new MemoryStream(payload.Data);
        using var reader = new BinaryReader(stream);

        if (reader.ReadByte() != START_BYTE)
        {
            return default;
        }

        if (reader.ReadByte() != (byte)SeStringChunkType.Interactable)
        {
            return default;
        }

        var length = reader.ReadByte();
        if (reader.ReadByte() != EmbeddedInfoTypeByte)
        {
            return default;
        }

        var result = new LifestreamPayload();
        result.DecodeImpl(reader, /* unused */ default);
        return result;
    }
}
