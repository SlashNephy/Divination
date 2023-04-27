using Dalamud.Divination.Common.Api.Memory;
using Dalamud.Game.ClientState.Objects.SubKinds;

namespace Divination.DiscordIntegration;

public static class PlayerCharacterUtils
{
    public static byte? GetIcon(this PlayerCharacter character)
    {
        var offset = DiscordIntegration.Instance.Divination.Definition?.Provider.Container.IconOffset;
        return !offset.HasValue ? default : character.ReadByte(offset.Value);
    }

    public static short? GetTitle(this PlayerCharacter character)
    {
        var offset = DiscordIntegration.Instance.Divination.Definition?.Provider.Container.TitleOffset;
        return !offset.HasValue ? default : character.ReadInt16(offset.Value);
    }
}
