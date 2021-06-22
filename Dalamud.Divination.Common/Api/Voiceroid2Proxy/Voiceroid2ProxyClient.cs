using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Logger;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Voiceroid2Proxy
{
    internal sealed class Voiceroid2ProxyClient : IVoiceroid2ProxyClient
    {
        private readonly string url;

        private readonly HttpClient client = new();
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(Voiceroid2ProxyClient));

        public Voiceroid2ProxyClient(string host = "localhost", int port = 4532)
        {
            url = $"http://{host}:{port}/talk";
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
                logger.Verbose("Talk: {Text}", text);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while TalkAsync");
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
