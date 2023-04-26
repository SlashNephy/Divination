namespace Divination.ACT.DiscordIntegration
{
    public class Settings : PluginSettings
    {
        public const long CheckIntervalMs = 3000;

        public Settings()
        {
            this.Bind("DetailsFormat", Plugin.TabControl.detailsTextBox);
            this.Bind("DetailsInCombatFormat", Plugin.TabControl.detailsInCombatTextBox);
            this.Bind("StateFormat", Plugin.TabControl.stateTextBox);
            this.Bind("LogoTooltipFormat", Plugin.TabControl.logoTooltipTextBox);
            this.Bind("TooltipFormat", Plugin.TabControl.tooltipTextBox);
            this.Bind("ShowJobIcon", Plugin.TabControl.jobIconCheckBox);
            this.Bind("CustomStatusFormat", Plugin.TabControl.customStatusTextBox);
            this.Bind("CustomStatusOnContentsFormat", Plugin.TabControl.customStatusOnContentsTextBox);
            this.Bind("ShowCustomStatus", Plugin.TabControl.customStatusCheckBox);
            this.Bind("ShowJobEmoji", Plugin.TabControl.jobEmojiCheckBox);
            this.Bind("ShowOnlineStatusEmoji", Plugin.TabControl.onlineStatusEmojiCheckBox);
            this.Bind("AuthorizationToken", Plugin.TabControl.authorizationTokenTextBox);
            this.Bind("ResetTimer", Plugin.TabControl.resetTimerCheckBox);
            this.Bind("RequireTargetingOnCombat", Plugin.TabControl.requireTargetingOnCombatCheckBox);
            this.Bind("CustomStatusDefaultText", Plugin.TabControl.customStatusTextDefaultTextBox);
            this.Bind("CustomStatusDefaultEmojiId", Plugin.TabControl.emojiIdTextBox);
        }

        public static string DetailsValue => Plugin.TabControl.detailsTextBox.Text;
        public static string DetailsInCombatValue => Plugin.TabControl.detailsInCombatTextBox.Text;
        public static string StateValue => Plugin.TabControl.stateTextBox.Text;
        public static string LogoTooltipValue => Plugin.TabControl.logoTooltipTextBox.Text;
        public static string TooltipValue => Plugin.TabControl.tooltipTextBox.Text;
        public static bool JobIconEnabled => Plugin.TabControl.jobIconCheckBox.Checked;
        public static string CustomStatusValue => Plugin.TabControl.customStatusTextBox.Text;
        public static string CustomStatusOnContentsValue => Plugin.TabControl.customStatusOnContentsTextBox.Text;

        public static bool CustomStatusEnabled => Plugin.TabControl.customStatusCheckBox.Checked &&
                                                  !string.IsNullOrWhiteSpace(AuthorizationTokenValue);

        public static bool JobEmojiEnabled => Plugin.TabControl.jobEmojiCheckBox.Checked;
        public static bool OnlineStatusEmojiEnabled => Plugin.TabControl.onlineStatusEmojiCheckBox.Checked;
        public static string AuthorizationTokenValue => Plugin.TabControl.authorizationTokenTextBox.Text;
        public static bool ResetTimerEnabled => Plugin.TabControl.resetTimerCheckBox.Checked;
        public static bool RequireTargetingOnCombat => Plugin.TabControl.requireTargetingOnCombatCheckBox.Checked;
        public static string CustomStatusDefaultTextValue => Plugin.TabControl.customStatusTextDefaultTextBox.Text;
        public static string CustomStatusDefaultEmojiIdValue => Plugin.TabControl.emojiIdTextBox.Text;
    }
}
