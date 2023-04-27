using Dalamud.Configuration;

namespace Divination.Voiceroid2Talker
{
    public class PluginConfig : IPluginConfiguration
    {
        public bool EnableTtsFcChatOnInactive;

        public int Version { get; set; }
    }
}
