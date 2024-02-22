using System.Collections.Generic;
using Newtonsoft.Json;

#pragma warning disable 8618

namespace Divination.SseClient.Handlers.MobHunt.Faloop.Api;

public class FaloopIdentifyResult
{
    [JsonProperty("success")] public bool Success { get; set; }

    [JsonProperty("session")] public SessionModel Session { get; set; }

    [JsonProperty("token")] public string Token { get; set; }

    public class SessionModel
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("verifyCodes")] public List<string> VerifyCodes { get; set; }
    }
}