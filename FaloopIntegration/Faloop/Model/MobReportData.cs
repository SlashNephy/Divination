using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Divination.FaloopIntegration.Faloop.Model;

public record MobReportData(
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("data")] JsonObject Data,
    [property: JsonPropertyName("id")] MobReportIds Ids,
    [property: JsonPropertyName("zoneInstance")] int ZoneInstance)
{
    public record Spawn(
        [property: JsonPropertyName("timestamp")] DateTime Timestamp,
        [property: JsonPropertyName("zoneId2")] string ZoneId,
        [property: JsonPropertyName("zonePoiIds")] List<int>? ZonePoiIds,
        [property: JsonPropertyName("reporters")] List<Reporter>? Reporters);

    public record Reporter([property: JsonPropertyName("name")] string Name);

    public record SpawnLocation(
        [property: JsonPropertyName("zoneId2")] string ZoneId,
        [property: JsonPropertyName("zonePoiId")] int ZonePoiId);

    public record SpawnRelease(
        [property: JsonConverter(typeof(UnixEpochMillisecondsTimeJsonConverter))]
        [property: JsonPropertyName("timestamp")] DateTime Timestamp);

    public record Death([property: JsonPropertyName("startedAt")] DateTime StartedAt);
}

public record MobReportIds(
    [property: JsonPropertyName("mobId")] string MobId,
    [property: JsonPropertyName("worldId")] string WorldId
);

public static class MobReportActions
{
    public const string Spawn = "spawn";
    public const string SpawnLocation = "spawn_location";
    public const string SpawnRelease = "spawn_release";
    public const string Death = "death";
}

public class UnixEpochMillisecondsTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.UnixEpoch.AddMilliseconds(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var epoch = value - DateTime.UnixEpoch;
        writer.WriteNumberValue((long)epoch.TotalMilliseconds);
    }
}
