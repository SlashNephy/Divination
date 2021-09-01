using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace Dalamud.Divination.Test
{
    public class AnotherPlugin : DivinationPlugin<AnotherPlugin, TestConfig>, IDalamudPlugin, ICommandSupport
    {
        public string CommandPrefix => "/another";
        public override Assembly Assembly => Assembly.GetExecutingAssembly();

        public AnotherPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ChatGui chatGui) : base(pluginInterface, chatGui)
        {
        }

        public static string ReadField()
        {
            return TestPlugin.Instance.SomeField;
        }
    }
}
