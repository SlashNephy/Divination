using Dalamud.Game.Text;

namespace Dalamud.Divination.Common.Api.Dalamud;

public static class SeIconCharEx
{
    public static string AsString(this SeIconChar icon)
    {
        return ((char)icon).ToString();
    }
}
