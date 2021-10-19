using Dalamud.Configuration;

namespace Divination.Debugger
{
    public class PluginConfig : IPluginConfiguration
    {
        public bool OpenAtStart = true;
        public bool EnableVerboseChatLog = false;

        public int Version { get; set; }
    }
}
