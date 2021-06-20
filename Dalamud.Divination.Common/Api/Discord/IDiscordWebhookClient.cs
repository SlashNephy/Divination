using System;
using System.Threading.Tasks;

namespace Dalamud.Divination.Common.Api.Discord
{
    public interface IDiscordWebhookClient : IDisposable
    {
        public Task SendAsync(string content, string? username = null, string? avatarUrl = null);
    }
}
