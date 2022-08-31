using Dalamud.Divination.Common.Api.Memory;
using Dalamud.Game.ClientState.Objects.SubKinds;

namespace Divination.DiscordIntegration
{
    public static class PlayerCharacterUtils
    {
        public static byte? GetIcon(this PlayerCharacter character)
        {
            return character.ReadByte(DiscordIntegrationPlugin.Instance.Divination.Definition?.Provider.Container.IconOffset);
        }

        public static short? GetTitle(this PlayerCharacter character)
        {
            return character.ReadInt16(DiscordIntegrationPlugin.Instance.Divination.Definition?.Provider.Container.TitleOffset);
        }
    }
}
