using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Divination.ChatFilter.Filters;

namespace Divination.ChatFilter
{
    public class ChatFilterManager : IDisposable
    {
        private readonly List<IChatFilter> filters = new()
        {
            new DebugMessageFilter(),
            new DuplicatedMapLinkFilter(),
            new DuplicatedMessageFilter(),
            new LsGreetingFilter(),
            new NoticeFilter(),
            new NoviceNetworkJoinMessageFilter(),
            new SonarRankBFilter()
        };

        public ChatFilterManager()
        {
            ChatFilterPlugin.Instance.Dalamud.ChatGui.ChatMessage += OnChatMessage;
        }

        private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (isHandled)
            {
                return;
            }

            foreach (var filter in filters.Where(x => x.IsAvailable()))
            {
                if (filter.Test(type, sender, message))
                {
                    isHandled = true;
                    return;
                }
            }
        }

        public void Dispose()
        {
            ChatFilterPlugin.Instance.Dalamud.ChatGui.ChatMessage -= OnChatMessage;

            foreach (var disposable in filters.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}
