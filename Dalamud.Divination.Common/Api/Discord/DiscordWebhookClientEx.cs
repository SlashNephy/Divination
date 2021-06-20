namespace Dalamud.Divination.Common.Api.Discord
{
    public static class DiscordWebhookClientEx
    {
        public static void Send(this IDiscordWebhookClient client, string content, string? username = null, string? avatarUrl = null)
        {
            client.SendAsync(content, username, avatarUrl).GetAwaiter().GetResult();
        }
    }
}
