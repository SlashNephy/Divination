using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;

namespace Divination.ChatFilter.Filters;

public interface IChatFilter
{
    public bool IsAvailable();

    public bool Test(XivChatType type, SeString sender, SeString message);
}