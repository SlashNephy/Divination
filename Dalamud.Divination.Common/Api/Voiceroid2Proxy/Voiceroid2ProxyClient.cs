using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Logging;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Voiceroid2Proxy
{
    internal sealed class Voiceroid2ProxyClient : IVoiceroid2ProxyClient
    {
        private readonly string url;

        private readonly HttpClient client = new();

        public Voiceroid2ProxyClient(string host = "localhost", int port = 4532)
        {
            url = $"http://{host}:{port.ToString()}/talk";
        }

        public async Task TalkAsync(string text)
        {
            var payload = new Dictionary<string, string>
            {
                {"text", text}
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            try
            {
                await client.PostAsync(url, content);
                PluginLog.Verbose("Talk: {Text}", text);
            }
            catch (Exception ex)
            {
                PluginLog.Error(ex, "Error occurred while TalkAsync");
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
