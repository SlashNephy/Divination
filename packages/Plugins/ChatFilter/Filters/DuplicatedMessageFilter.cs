using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.ChatFilter.Filters;

public class DuplicatedMessageFilter : IChatFilter
{
    private const XivChatType PartyRecruitingChatType = (XivChatType) 72;
    private readonly XivChatType[] checkDuplicationTypes =
    {
        XivChatType.Say, XivChatType.Shout, XivChatType.Yell, XivChatType.FreeCompany,
        XivChatType.Ls1, XivChatType.Ls2, XivChatType.Ls3, XivChatType.Ls4,
        XivChatType.Ls5, XivChatType.Ls6, XivChatType.Ls7, XivChatType.Ls8,
        XivChatType.CrossLinkShell1, XivChatType.CrossLinkShell2, XivChatType.CrossLinkShell3, XivChatType.CrossLinkShell4,
        XivChatType.CrossLinkShell5, XivChatType.CrossLinkShell6, XivChatType.CrossLinkShell7, XivChatType.CrossLinkShell8,
        XivChatType.ErrorMessage, PartyRecruitingChatType
    };
    private readonly XivChatType[] skipDuplicationCheckOnSender =
    {
        // Shout/Yell/Say は sender をいじるので本文だけチェックする
        XivChatType.Shout, XivChatType.Yell, XivChatType.Say
    };

    private readonly Dictionary<XivChatType, (string sender, string message, DateTime time)> messageLogs = new();
    private readonly TimeSpan duplicationDisallowedSpan = TimeSpan.FromMinutes(5);

    public bool IsAvailable() => ChatFilterPlugin.Instance.Config.EnableDuplicatedMessageFilter;

    public bool Test(XivChatType type, SeString sender, SeString message)
    {
        if (!ShouldCheckDuplication(type))
        {
            return false;
        }

        lock (messageLogs)
        {
            var (senderText, messageText) = (sender.TextValue, message.TextValue);

            try
            {
                if (messageLogs.TryGetValue(type, out var value))
                {
                    var (lastSender, lastMessage, lastTime) = value;

                    return (skipDuplicationCheckOnSender.Contains(type) || lastSender == senderText) && lastMessage == messageText && DateTime.Now - lastTime < duplicationDisallowedSpan;
                }
            }
            finally
            {
                messageLogs[type] = (senderText, messageText, DateTime.Now);
            }
        }

        return false;
    }

    private bool ShouldCheckDuplication(XivChatType type)
    {
        return checkDuplicationTypes.Contains(type) || type == ChatFilterPlugin.Instance.Config.LsMessagesType || type == Config.CwlsMessagesType || (Config.FilterEchoDuplication && type == XivChatType.Echo);
    }
}