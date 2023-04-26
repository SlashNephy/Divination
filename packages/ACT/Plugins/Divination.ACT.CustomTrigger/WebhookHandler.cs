using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Divination.Common;

#nullable enable
namespace Divination.ACT.CustomTrigger
{
    public static class WebhookHandler
    {
        private const string MobHuntWebhookUrl =
            "https://discordapp.com/api/webhooks/<reducted>";

        private const string GeneralVcWebhookUrl =
            "https://discordapp.com/api/webhooks/<reducted>";

        private static readonly List<string> IgnoreWords = new List<string>
        {
            "ありがと", "おはよ", "こんにち", "こんばん", "おやすみ", "よろしく", "おつかれ", "お疲れ", "こんちゃ", "ありでした", "感謝です", "ごちでした", "ごちそう",
            "ご馳走", "御馳走"
        };

        public static async Task SendToMobHunt(string username, string text, string linkshell)
        {
            if (IgnoreWords.Any(text.Contains))
            {
                return;
            }

            var content = text.ReplaceSpecialCharacters();
            var world = username.ExtractWorldName();
            var player = username.RemoveUnicodePrefix().RemoveSuffix(world);
            var avatarUrl = await Api.GetAvatarUrlAsync(player, world);

            await Plugin.PostDiscordWebhook(MobHuntWebhookUrl, content, $"[{linkshell}] {player}@{world}", avatarUrl);
        }

        public static async Task SendToWorldSatellite(string username, string text, string? avatarUrl = null)
        {
            await Plugin.PostDiscordWebhook(MobHuntWebhookUrl, text, username, avatarUrl);
        }

        public static async Task SendToGeneralVc(string username, string text)
        {
            var content = text.ReplaceSpecialCharacters();
            var player = username.RemoveUnicodePrefix();
            var avatarUrl = await Api.GetAvatarUrlAsync(player, "Valefor");

            await Plugin.PostDiscordWebhook(GeneralVcWebhookUrl, content, $"[FC] {player}", avatarUrl);
        }

        private static class Api
        {
            private static readonly TimeSpan Expired = TimeSpan.FromHours(6);
            private static readonly Dictionary<string, string> AvatarUrlCache = new Dictionary<string, string>();
            private static readonly Dictionary<string, DateTime> LastCacheTime = new Dictionary<string, DateTime>();

            public static async Task<string?> GetAvatarUrlAsync(string name, string world)
            {
                var key = $"{name}@{world}";

                if (AvatarUrlCache.TryGetValue(key, out var avatarUrl) &&
                    LastCacheTime.TryGetValue(key, out var time) && DateTime.Now - time < Expired)
                {
                    return avatarUrl;
                }

                try
                {
                    var response = await Plugin.XivApi.GetCharacterAsync(name, world, true);
                    avatarUrl = response.Dynamic.Avatar;
                    AvatarUrlCache[key] = avatarUrl;
                    LastCacheTime[key] = DateTime.Now;

                    return avatarUrl;
                }
                catch (Exception e)
                {
                    Plugin.Logger.Error(e);
                }

                return null;
            }
        }
    }
}
