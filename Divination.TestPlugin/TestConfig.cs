using Dalamud.Configuration;

namespace Divination.TestPlugin
{
    public class TestConfig : IPluginConfiguration
    {
        public int Version { get; set; } = 1;
    }
}
