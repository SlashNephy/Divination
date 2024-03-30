using System.Text.Json.Serialization;

namespace FaloopApi.Faloop.Model;
public record LoginData(string SessionId, string Token);
public record UserLoginResponse(
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("data")] LoginData Data);
