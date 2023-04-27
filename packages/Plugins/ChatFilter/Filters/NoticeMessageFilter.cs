using System;
using System.Text.RegularExpressions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.ChatFilter.Filters
{
    public class NoticeFilter : IChatFilter
    {
        private bool firstNoticeReceived = true;
        private DateTime? lastNoticeReceived;
        private readonly Regex welcomePattern = new Regex("Welcome to \\w+!", RegexOptions.Compiled);

        public bool IsAvailable() => ChatFilterPlugin.Instance.Config.EnableNoticeMessageFilter;

        public bool Test(XivChatType type, SeString sender, SeString message)
        {
            if (type != XivChatType.Notice)
            {
                return false;
            }

            // ログイン後最初のメッセージは対象外にする
            if (firstNoticeReceived)
            {
                firstNoticeReceived = false;
            }
            // 「Welcome to xxx!」を見つけた
            else if (welcomePattern.IsMatch(message.TextValue))
            {
                lastNoticeReceived = DateTime.Now;
            }

            // Notice メッセージを以前見つけて 10秒以内
            return DateTime.Now - lastNoticeReceived < TimeSpan.FromSeconds(10);
        }
    }
}
