using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Divination.FaloopIntegration.Faloop.Model;

public record FaloopEventPayload(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("subType")] string SubType,
    [property: JsonPropertyName("data")] JsonObject Data);
