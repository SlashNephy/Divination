namespace Dalamud.Divination.Common.Api.Voiceroid2Proxy;

public static class Voiceroid2ProxyClientEx
{
    public static void Talk(this IVoiceroid2ProxyClient client, string text)
    {
        client.TalkAsync(text).GetAwaiter().GetResult();
    }
}
