using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Logging;
using Divination.ACT.DiscordIntegration.Data;
using Divination.Common;
using Newtonsoft.Json;
using LogLevel = DiscordRPC.Logging.LogLevel;

#nullable enable
namespace Divination.ACT.DiscordIntegration
{
    internal static class DiscordApi
    {
        private const string ClientId = "745518092263620658";

        private static DiscordRpcClient? _rpcClient;
        private static Timestamps? _startTime;
        private static string? _lastCustomStatusEmojiId;
        private static string? _lastCustomStatusText;

        private static bool CreateClient()
        {
            if (_rpcClient?.IsDisposed == false)
            {
                return true;
            }

            _rpcClient = new DiscordRpcClient(ClientId)
            {
                Logger = new DiscordRPC.Logging.ConsoleLogger(LogLevel.Warning),
                SkipIdenticalPresence = true
            };
            _rpcClient.OnPresenceUpdate += (sender, args) =>
            {
                Plugin.Logger.Trace(
                    "RichPresence:" +
                    $"Details        = {args.Presence.Details}",
                    $"State          = {args.Presence.State}",
                    $"SmallImageText = {args.Presence.Assets.SmallImageText}",
                    $"LargeImageText = {args.Presence.Assets.LargeImageText}");
            };

            _startTime = Timestamps.Now;

            return _rpcClient.Initialize();
        }

        public static void UpdatePresence(RichPresence presence, bool resetTimer)
        {
            if (!CreateClient())
            {
                return;
            }

            if (resetTimer)
            {
                _startTime = Timestamps.Now;
            }
            presence.Timestamps = _startTime;

            _rpcClient?.SetPresence(presence);
            if (_rpcClient?.AutoEvents == false)
            {
                _rpcClient?.Invoke();
            }
        }

        public static async Task UpdateCustomStatus(Emoji? emoji, string? text)
        {
            static string? ValueOrNull(string? value)
            {
                return string.IsNullOrEmpty(value) ? null : value;
            }

            await UpdateCustomStatus(
                ValueOrNull(emoji?.Id ?? Settings.CustomStatusDefaultEmojiIdValue),
                emoji?.Name,
                ValueOrNull(text ?? Settings.CustomStatusDefaultTextValue)
            );
        }

        private static async Task UpdateCustomStatus(string? emojiId, string? emojiName, string? text)
        {
            if (emojiId == _lastCustomStatusEmojiId && text == _lastCustomStatusText)
            {
                return;
            }

            var payload = new Dictionary<dynamic, dynamic>
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

            const string url = "https://discord.com/api/v8/users/@me/settings";
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Accept-Language", "ja");
            request.Headers.Add("Authorization", Settings.AuthorizationTokenValue);
            request.Headers.Add("Origin", "https://discord.com");

            await Plugin.HttpClient.SendAsync(request);

            _lastCustomStatusEmojiId = emojiId;
            _lastCustomStatusText = text;

            Plugin.Logger.Trace($"CustomStatus: :{emojiName ?? "null"}: ({emojiId ?? "null"}) / {text ?? "null"}");
        }

        public static void Release()
        {
            _rpcClient?.Dispose();

            Task.Run(async () =>
            {
                try
                {
                    await UpdateCustomStatus(null, null);
                }
                catch (Exception e)
                {
                    Plugin.Logger.Error(e);
                }
            });
        }
    }
}
