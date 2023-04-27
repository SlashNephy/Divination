using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.ChatFilter.Filters;

public class SonarRankBFilter : IChatFilter
{
    public bool IsAvailable() => ChatFilterPlugin.Instance.Config.EnableSonarRankBFilter;

    public bool Test(XivChatType type, SeString sender, SeString message)
    {
        return sender.TextValue == "Sonar" && message.TextValue.StartsWith("Rank B:");
    }
}