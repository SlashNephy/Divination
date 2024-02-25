using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Dalamud;
using DiscordRPC;
using DiscordRPC.Helper;
using Newtonsoft.Json;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace Divination.DiscordIntegration.Discord;

public sealed class DiscordApi : IDisposable
{
    private const string ClientId = "745518092263620658";

    private DiscordRpcClient? rpcClient;
    private readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());
    private string? previousCustomStatusEmojiId;
    private string? previousCustomStatusEmojiName;
    private string? previousCustomStatusText;

    private bool CreateRpcClient()
    {
        if (rpcClient?.IsDisposed == false)
        {
            return true;
        }

        rpcClient = new DiscordRpcClient(ClientId)
        {
            Logger = new DiscordRpcLogger(LogLevel.Info),
            SkipIdenticalPresence = true
        };
        rpcClient.OnPresenceUpdate += (_, args) =>
        {
            DalamudLog.Log.Verbose(
                "RichPresence:\nDetails        = {Details}\nState          = {State}\nSmallImageText = {SmallImageText}\nLargeImageText = {LargeImageText}",
                args.Presence.Details, args.Presence.State, args.Presence.Assets.SmallImageText, args.Presence.Assets.LargeImageText);
        };
        rpcClient.OnConnectionFailed += (_, _) =>
        {
            rpcClient.Dispose();
        };

        return rpcClient.Initialize();
    }

    public void UpdatePresence(RichPresence presence)
    {
        if (!CreateRpcClient())
        {
            return;
        }

        rpcClient?.SetPresence(presence);
        if (rpcClient?.AutoEvents == false)
        {
            rpcClient?.Invoke();
        }
    }

    public async Task UpdateCustomStatus(string? emojiName, string? emojiId, string? text)
    {
        emojiName ??= DiscordIntegration.Instance.Config.CustomStatusDefaultEmojiName.GetNullOrString();
        emojiId ??= DiscordIntegration.Instance.Config.CustomStatusDefaultEmojiId.GetNullOrString();
        text ??= DiscordIntegration.Instance.Config.CustomStatusDefaultText.GetNullOrString();

        if (emojiId == previousCustomStatusEmojiId && emojiName == previousCustomStatusEmojiName && text == previousCustomStatusText)
        {
            return;
        }

        var token = DiscordIntegration.Instance.Config.AuthorizationToken;
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        const string url = "https://discord.com/api/v8/users/@me/settings";
        var payload = new Dictionary<string, object>
        {
            {
                "custom_status", new Dictionary<string, string?>
                {
                    {"emoji_id", emojiId},
                    {"emoji_name", emojiName},
                    {"text", text}
                }
            }
        };
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
        {
            Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
        };

        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Language", "ja");
        request.Headers.Add("Authorization", token);
        request.Headers.Add("Origin", "https://discord.com");

        try
        {
            await httpClient.Value.SendAsync(request);
        }
        catch (Exception ex)
        {
            DalamudLog.Log.Error(ex, "Failed to send request");
            return;
        }

        previousCustomStatusEmojiId = emojiId;
        previousCustomStatusEmojiName = emojiName;
        previousCustomStatusText = text;

        DalamudLog.Log.Verbose("CustomStatus: :{EmojiName}: ({EmojiId} / {Text})", emojiName ?? "null", emojiId ?? "null", text ?? "null");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            rpcClient?.Dispose();
            if (httpClient.IsValueCreated)
            {
                httpClient.Value.Dispose();
            }

            UpdateCustomStatus(null, null, null).Wait();
        }
    }

    ~DiscordApi()
    {
        Dispose(false);
    }
}
