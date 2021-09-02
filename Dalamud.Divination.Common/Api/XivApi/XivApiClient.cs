using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Logger;
using Newtonsoft.Json.Linq;

namespace Dalamud.Divination.Common.Api.XivApi
{
    internal sealed class XivApiClient : IXivApiClient
    {
        private readonly string? apiKey;

        private readonly HttpClient client = new();
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(XivApiClient));

        public XivApiClient(string? apiKey = null)
        {
            this.apiKey = apiKey;
        }

        public async Task<XivApiResponse> GetAsync(string content, uint id, bool ignoreCache = false)
        {
            var key = id.ToString();
            var url = $"https://xivapi.com/{content}/{key}";
            if (apiKey != null)
            {
                url += $"&private_key={apiKey}";
            }

            using var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(result);

            logger.Verbose("{Code}: {Method} {Url}", (int) response.StatusCode, response.RequestMessage!.Method.Method, url);

            return new XivApiResponse(data);
        }

        public async Task<XivApiResponse> GetCharacterAsync(string name, string world, bool ignoreCache = false)
        {
            var url = $"https://xivapi.com/character/search?name={Uri.EscapeUriString(name)}&server={world}";
            if (apiKey != null)
            {
                url += $"&private_key={apiKey}";
            }

            using var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            dynamic json = JObject.Parse(result);
            var data = (JObject) ((JArray) json.Results).First();

            logger.Verbose("{Code}: {Method} {Url}", (int) response.StatusCode, response.RequestMessage!.Method.Method, url);
            return new XivApiResponse(data);
        }

        public static string GetIconUrl(uint icon)
        {
            string folderId;
            var iconId = icon.ToString();

            if (iconId.Length >= 6)
            {
                iconId = iconId.PadLeft(5, '0');
                folderId = string.Join("", iconId.Take(3)) + "000";
            }
            else
            {
                iconId = "0" + iconId.PadLeft(5, '0');
                folderId = "0" + string.Join("", iconId.Skip(1).Take(2)) + "000";
            }

            return $"https://xivapi.com/i/{folderId}/{iconId}.png";
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
