using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;

namespace Divination.TestPlugin
{
    public class TestPlugin : DivinationPlugin<TestPlugin, TestConfig>, IDalamudPlugin, ICommandSupport, IConfigWindowSupport<TestConfig>, IDefinitionSupport
    {
        public TestPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Logger.Information("Plugin Loaded!");
        }

        public string MainCommandPrefix => "/test";
        public ConfigWindow<TestConfig> CreateConfigWindow() => new TestPluginConfigWindow();

        public string DefinitionUrl => "https://example.com/def.json";
    }
}
