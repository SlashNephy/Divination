using Dalamud.Configuration;

namespace Divination.Template
{
    public class PluginConfig : IPluginConfiguration
    {
        public bool Enabled;

        public int Version { get; set; }
    }
}
