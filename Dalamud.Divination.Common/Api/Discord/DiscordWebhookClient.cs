using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dalamud.Divination.Common.Api.Discord
{
    public sealed class DiscordWebhookClient : IDiscordWebhookClient
    {
        private readonly string url;

        private readonly HttpClient client = new();

        public DiscordWebhookClient(string url)
        {
            this.url = url;
        }

        public async Task SendAsync(DiscordWebhookMessage message)
        {
            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(url, content);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
