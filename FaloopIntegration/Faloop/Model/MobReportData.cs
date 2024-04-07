using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Divination.FaloopIntegration.Faloop.Model;

public record MobReportData(
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("data")] JsonObject Data,
    [property: JsonPropertyName("mobId")] uint MobId,
    [property: JsonPropertyName("worldId")] uint WorldId,
    [property: JsonPropertyName("zoneInstance")] int ZoneInstance)
{
    public record Spawn(
        [property: JsonPropertyName("timestamp")] DateTime Timestamp,
        [property: JsonPropertyName("zoneId")] uint ZoneId,
        [property: JsonPropertyName("zonePoiIds")] List<int>? ZonePoiIds,
        [property: JsonPropertyName("reporters")] List<Reporter>? Reporters);

    public record Reporter([property: JsonPropertyName("name")] string Name);

    public record Death(
        [property: JsonPropertyName("num")] int Num,
        [property: JsonPropertyName("startedAt")] DateTime StartedAt);
}
