using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Divination.SseClient.Handlers.MobHunt.Faloop.Api;

public sealed class FaloopApiClient : IDisposable
{
    private readonly HttpClient httpClient = new();

    private async Task<FaloopIdentifyResult> Identify()
    {
        var message = new HttpRequestMessage(HttpMethod.Post, "https://api.faloop.app/api/auth/user/identify")
        {
            Content = new StringContent("{\"sessionId\":\"\"}", Encoding.UTF8, "application/json"),
            Headers =
            {
                { "Accept", "application/json, text/plain, */*" },
                { "AcceptEncoding", "gzip, deflate, br" },
                { "AcceptLanguage", "ja" },
                { "Origin", "https://faloop.app" },
                { "Referrer", "https://faloop.app/" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.0.0 Safari/537.36" }
            }
        };

        var response = await httpClient.SendAsync(message);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<FaloopIdentifyResult>(content)!;
        if (!result.Success)
        {
            throw new AggregateException("Request for /identify has failed.");
        }

        return result;
    }

    public async Task<FaloopInitResult> Init()
    {
        var identify = await Identify();

        var message = new HttpRequestMessage(HttpMethod.Post, "https://api.faloop.app/api/init")
        {
            Content = new StringContent($"{{\"sessionId\":\"{identify.Session.Id}\"}}", Encoding.UTF8, "application/json"),
            Headers =
            {
                { "Accept", "application/json, text/plain, */*" },
                { "AcceptEncoding", "gzip, deflate, br" },
                { "AcceptLanguage", "ja" },
                { "Authorization", identify.Token },
                { "Origin", "https://faloop.app" },
                { "Referrer", "https://faloop.app/" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.0.0 Safari/537.36" }
            }
        };
        var response = await httpClient.SendAsync(message);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<FaloopInitResult>(content)!;
        if (!result.Success)
        {
            throw new AggregateException("Request for /init has failed.");
        }

        return result;
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }
}