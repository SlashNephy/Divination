using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.ChatFilter.Filters;

public class NoviceNetworkJoinMessageFilter : IChatFilter
{
    public bool IsAvailable() => ChatFilterPlugin.Instance.Config.EnableNoviceNetworkJoinMessagesFilter;

    public bool Test(XivChatType type, SeString sender, SeString message)
    {
        return (ushort) type == 75;
    }
}