using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.ChatFilter.Filters
{
    public class DebugMessageFilter : IChatFilter
    {
        public bool IsAvailable() => ChatFilterPlugin.Instance.Config.EnableDebugMessageFilter;

        public bool Test(XivChatType type, SeString sender, SeString message)
        {
            if (type != XivChatType.Debug)
            {
                return false;
            }

            if (ChatFilterPlugin.Instance.Config.MoveDebugMessagesToEcho)
            {
                ChatFilterPlugin.Instance.Divination.Chat.EnqueueChat(new XivChatEntry
                {
                    Type = XivChatType.Echo,
                    Name = sender,
                    Message = message
                });
            }

            return true;
        }
    }
}
