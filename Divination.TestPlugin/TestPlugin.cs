using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;

namespace Divination.TestPlugin
{
    public class TestPlugin : DivinationPlugin<TestPlugin, TestConfig>, IDalamudPlugin, ICommandSupport
    {
        public string CommandPrefix => "/test";

        public TestPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Logger.Information("Plugin Loaded!");
        }
    }
}
