using Dalamud.Configuration;

namespace Divination.AetheryteLinkInChat
{
    public class PluginConfig :IPluginConfiguration
    {
        public bool Enabled;

        public int Version { get; set; }
    }
}
