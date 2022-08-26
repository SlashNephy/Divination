using System;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Discord
{
    public interface IDiscordWebhookClient : IDisposable
    {
        public Task SendAsync(DiscordWebhookMessage message);
    }
}
