using System;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Lumina.Excel.Sheets;

namespace Divination.AetheryteLinkInChat.Payloads;

public sealed class AetherytePayload : DalamudLinkPayload
{
    // 未使用だと思われる
    internal const byte EmbeddedInfoTypeByte = (byte)(EmbeddedInfoType.DalamudLink + 1);

    public uint AetheryteId { get; set; }
    public Aetheryte Aetheryte => AetheryteLinkInChat.Instance.Dalamud.DataManager.GetExcelSheet<Aetheryte>().HasRow(AetheryteId)
        ? AetheryteLinkInChat.Instance.Dalamud.DataManager.GetExcelSheet<Aetheryte>().GetRow(AetheryteId)
        : throw new InvalidOperationException("invalid aetheryte ID");

    public override PayloadType Type => PayloadType.Unknown;

    public AetherytePayload(Aetheryte aetheryte)
    {
        AetheryteId = aetheryte.RowId;
    }

    private AetherytePayload()
    {
    }

    protected override byte[] EncodeImpl()
    {
        var data = MakeInteger(AetheryteId);
        var length = 2 + (byte)data.Length;

        return
        [
            START_BYTE,
            (byte)SeStringChunkType.Interactable,
            (byte)length,
            EmbeddedInfoTypeByte,
            .. data,
            END_BYTE
        ];
    }

    protected override void DecodeImpl(BinaryReader reader, long _)
    {
        AetheryteId = GetInteger(reader);
    }

    public override string ToString()
    {
        return $"{nameof(AetherytePayload)}[{AetheryteId}]";
    }

    public RawPayload ToRawPayload()
    {
        return new RawPayload(EncodeImpl());
    }

    public static AetherytePayload? Parse(RawPayload payload)
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

        var aetheryte = new AetherytePayload();
        aetheryte.DecodeImpl(reader, /* unused */ default);
        return aetheryte;
    }
}
