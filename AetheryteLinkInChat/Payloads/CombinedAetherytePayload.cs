using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Divination.AetheryteLinkInChat.Solver;

namespace Divination.AetheryteLinkInChat.Payloads;

public sealed class CombinedAetherytePayload(ITeleportPath[] paths) : DalamudLinkPayload
{
    private const byte EmbeddedInfoTypeByte = AetherytePayload.EmbeddedInfoTypeByte + 1;

    public ITeleportPath[] Paths => paths;

    public override PayloadType Type => PayloadType.Unknown;

    private CombinedAetherytePayload() : this([])
    {
    }

    protected override byte[] EncodeImpl()
    {
        var data = new List<byte>();
        foreach (var path in paths)
        {
            switch (path)
            {
                case AetheryteTeleportPath aetheryte:
                    var payload = new AetheryteTeleportPathPayload(aetheryte);
                    data.AddRange(payload.Encode());
                    continue;
                case BoundaryTeleportPath boundary:
                    var payload2 = new BoundaryTeleportPathPayload(boundary);
                    data.AddRange(payload2.Encode());
                    continue;
                case WorldTeleportPath world:
                    var payload3 = new WorldTeleportPathPayload(world);
                    data.AddRange(payload3.Encode());
                    continue;
                default:
                    throw new ArgumentOutOfRangeException("invalid path type");
            }
        }

        var length = 3 + (byte)data.Count;
        return [
            START_BYTE,
            (byte)SeStringChunkType.Interactable,
            (byte)length,
            EmbeddedInfoTypeByte,
            (byte)Paths.Length,
            .. data,
            END_BYTE,
        ];
    }

    protected override void DecodeImpl(BinaryReader reader, long _)
    {
        var length = reader.ReadByte();
        paths = new ITeleportPath[length];
        for (var i = 0; i < paths.Length; i++)
        {
            var marker = reader.ReadByte();
            switch (marker)
            {
                case AetheryteTeleportPathPayload.Marker:
                    var payload = new AetheryteTeleportPathPayload(null);
                    payload.Decode(reader);
                    paths[i] = payload.Path!;
                    continue;
                case BoundaryTeleportPathPayload.Marker:
                    var payload2 = new BoundaryTeleportPathPayload(null);
                    payload2.Decode(reader);
                    paths[i] = payload2.Path!;
                    continue;
                case WorldTeleportPathPayload.Marker:
                    var payload3 = new WorldTeleportPathPayload(null);
                    payload3.Decode(reader);
                    paths[i] = payload3.Path!;
                    continue;
                default:
                    throw new InvalidOperationException($"invalid marker: {marker}");
            }
        }
    }

    public override string ToString()
    {
        return $"{nameof(CombinedAetherytePayload)}[{paths}]";
    }

    public RawPayload ToRawPayload()
    {
        return new RawPayload(EncodeImpl());
    }

    public static CombinedAetherytePayload? Parse(RawPayload payload)
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

        var result = new CombinedAetherytePayload();
        result.DecodeImpl(reader, /* unused */ default);
        return result;
    }
}
