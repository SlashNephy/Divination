using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Divination.TestPlugin
{
    public class TestPlugin : DivinationPlugin<TestPlugin, TestConfig>, IDalamudPlugin, ICommandSupport
    {
        public string CommandPrefix => "/test";
        public override Assembly Assembly => Assembly.GetExecutingAssembly();

        public TestPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ChatGui chatGui) : base(pluginInterface, chatGui)
        {
            Logger.Information("Plugin Loaded!");
        }
    }
}
