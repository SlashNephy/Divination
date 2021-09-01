using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Dalamud.Divination.Test
{
    public class TestPlugin : DivinationPlugin<TestPlugin, TestConfig>
    {
        public readonly string SomeField = "Hello, World!";

        public override string Name => "Divination.TestPlugin";

        public TestPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ChatGui chatGui) : base(pluginInterface, chatGui)
        {
        }

        public override void DisposeManaged()
        {
            Logger.Information("Unloaded!");
        }
    }
}
