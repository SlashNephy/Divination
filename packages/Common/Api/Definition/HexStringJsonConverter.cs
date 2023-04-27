using System;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Definition;

public sealed class HexStringJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        objectType = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return objectType == typeof(byte) || objectType == typeof(ushort) || objectType == typeof(int);
    }

    public override object? ReadJson(JsonReader reader,
        Type objectType,
        object? existingValue,
        JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.String:
                var value = (string)reader.Value!;
                return Cast(objectType, Convert.ToInt32(value, 16));
            case JsonToken.Integer:
                return Cast(objectType, Convert.ToInt32(reader.Value));
            case JsonToken.Null:
                return null;
            default:
                throw new NotSupportedException(
                    $"Deserialization of \"{reader.Value}\" ({reader.TokenType}) is not supported. (expected {objectType}, {existingValue})");
        }
    }

    private static object Cast(Type objectType, int value)
    {
        var type = Nullable.GetUnderlyingType(objectType) ?? objectType;

        if (type == typeof(byte))
        {
            return Convert.ToByte(value);
        }

        if (type == typeof(int))
        {
            return value;
        }

        if (type == typeof(ushort))
        {
            return Convert.ToUInt16(value);
        }

        throw new NotSupportedException($"Casting {value} ({value.GetType()}) to {type} was failed.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue($"0x{value:X}");
    }
}
