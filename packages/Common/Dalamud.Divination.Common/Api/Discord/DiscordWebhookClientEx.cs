namespace Dalamud.Divination.Common.Api.Discord;

public static class DiscordWebhookClientEx
{
    public static void Send(this IDiscordWebhookClient client, DiscordWebhookMessage message)
    {
        client.SendAsync(message).GetAwaiter().GetResult();
    }
}
