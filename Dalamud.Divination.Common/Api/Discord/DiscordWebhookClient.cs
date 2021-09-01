using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Discord
{
    internal sealed class DiscordWebhookClient : IDiscordWebhookClient
    {
        private readonly string url;

        private readonly HttpClient client = new();

        public DiscordWebhookClient(string url)
        {
            this.url = url;
        }

        public async Task SendAsync(string content, string? username = null, string? avatarUrl = null)
        {
            var parameters = new Dictionary<string, string?>
            {
                {"content", content},
                {"username", username},
                {"avatar_url", avatarUrl}
            };
            var payload = new FormUrlEncodedContent(parameters!);

            await client.PostAsync(url, payload);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
