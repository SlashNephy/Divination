using Dalamud.Configuration;

namespace Divination.AetheryteLinkInChat
{
    public class Configuration : IPluginConfiguration
    {
        public bool Enabled = true;

        public int Version { get; set; }
    }
}
