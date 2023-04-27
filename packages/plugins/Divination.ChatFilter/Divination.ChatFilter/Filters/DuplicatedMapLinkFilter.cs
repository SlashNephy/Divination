using System;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Divination.ChatFilter.Filters
{
    public class DuplicatedMapLinkFilter : IChatFilter
    {
        private readonly XivChatType[] checkMapLinkDuplicationTypes =
        {
            XivChatType.Say, XivChatType.Shout, XivChatType.Yell,
            XivChatType.Ls1, XivChatType.Ls2, XivChatType.Ls3, XivChatType.Ls4,
            XivChatType.Ls5, XivChatType.Ls6, XivChatType.Ls7, XivChatType.Ls8,
            XivChatType.CrossLinkShell1, XivChatType.CrossLinkShell2, XivChatType.CrossLinkShell3, XivChatType.CrossLinkShell4,
            XivChatType.CrossLinkShell5, XivChatType.CrossLinkShell6, XivChatType.CrossLinkShell7, XivChatType.CrossLinkShell8
        };

        private readonly object lastMapLinkLock = new();
        private (MapLinkPayload map, DateTime time) lastMapLink;

        public bool IsAvailable() => ChatFilterPlugin.Instance.Config.FilterSameMapLinks;

        public bool Test(XivChatType type, SeString sender, SeString message)
        {
            if (!ShouldCheckMapLinkDuplication(type))
            {
                return false;
            }

            var mapLink = message.Payloads.OfType<MapLinkPayload>().FirstOrDefault();
            if (mapLink == null)
            {
                return false;
            }

            lock (lastMapLinkLock)
            {
                try
                {
                    if (lastMapLink != default)
                    {
                        var (lastMap, lastTime) = lastMapLink;

                        return lastMap.PlaceName == mapLink.PlaceName && lastMap.CoordinateString == mapLink.CoordinateString && DateTime.Now - lastTime < duplicationDisallowedSpan;
                    }
                }
                finally
                {
                    lastMapLink = (mapLink, DateTime.Now);
                }
            }

            return false;
        }

        private bool ShouldCheckMapLinkDuplication(XivChatType type)
        {
            return checkMapLinkDuplicationTypes.Contains(type) || type == Config.LsMessagesType || type == Config.CwlsMessagesType;
        }
    }
}
