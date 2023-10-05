using System.Collections.Generic;
using System.IO;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public sealed class AetherytePayload : DalamudLinkPayload
{
    // 未使用だと思われる
    private const byte EmbeddedInfoTypeByte = (byte)(EmbeddedInfoType.DalamudLink + 1);

    private uint AetheryteId { get; set; }
    private Aetheryte? Aetheryte => DataResolver.GetExcelSheet<Aetheryte>()?.GetRow(AetheryteId);

    public override PayloadType Type => PayloadType.Unknown;
    public RawPayload ToRawPayload() => new RawPayload(this.EncodeImpl());

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
        var bytes = new List<byte>
        {
            START_BYTE,
            (byte) SeStringChunkType.Interactable,
        };
        bytes.AddRange(MakeInteger((uint)data.Length + 1));
        bytes.Add(EmbeddedInfoTypeByte);
        bytes.AddRange(data);
        bytes.Add(END_BYTE);

        return bytes.ToArray();
    }

    protected override void DecodeImpl(BinaryReader reader, long endOfStream)
    {
        AetheryteId = GetInteger(reader);
    }

    public static Aetheryte? Parse(RawPayload payload)
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

        var length = GetInteger(reader);
        if (reader.ReadByte() != EmbeddedInfoTypeByte)
        {
            return default;
        }

        var aetheryte = new AetherytePayload();
        aetheryte.DecodeImpl(reader, reader.BaseStream.Position + length - 1L);
        return aetheryte.Aetheryte;
    }
}
