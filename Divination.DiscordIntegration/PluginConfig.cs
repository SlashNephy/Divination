using Dalamud.Configuration;

namespace Divination.DiscordIntegration
{
    public class PluginConfig : IPluginConfiguration
    {
        public string DetailsFormat = string.Empty;
        public string DetailsInOnlineFormat = string.Empty;
        public string DetailsInDutyFormat = string.Empty;
        public string DetailsInCombatFormat = string.Empty;
        public string StateFormat = string.Empty;
        public bool ShowJobSmallImage = true;
        public string SmallImageTextFormat = string.Empty;
        public bool ShowLoadingLargeImage = true;
        public string LargeImageTextFormat = string.Empty;
        public bool ResetTimerOnAreaChange = true;
        public bool RequireTargetingOnCombat = true;

        public bool ShowCustomStatus = false;
        public string AuthorizationToken = string.Empty;
        public bool ShowJobCustomStatusEmoji = true;
        public bool ShowOnlineStatusCustomStatusEmoji = true;
        public string CustomStatusFormat = string.Empty;
        public string CustomStatusInDutyFormat = string.Empty;
        public string CustomStatusDefaultEmojiId = string.Empty;
        public string CustomStatusDefaultEmojiName = string.Empty;
        public string CustomStatusDefaultText = string.Empty;

        public int Version { get; set; } = 0;
    }
}
