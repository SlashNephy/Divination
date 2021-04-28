using Dalamud.Configuration;

namespace Dalamud.Divination.Test
{
    public class TestConfig : IPluginConfiguration
    {
        public int Version { get; set; } = 1;
    }
}
