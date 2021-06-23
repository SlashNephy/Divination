using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;

namespace Dalamud.Divination.Test
{
    public class AnotherPlugin : DivinationPlugin<AnotherPlugin, TestConfig>, IDalamudPlugin, ICommandSupport
    {
        public override string Name => "AnotherPlugin";
        public string CommandPrefix => "/another";
        public override Assembly Assembly => Assembly.GetExecutingAssembly();

        public static string ReadField()
        {
            return TestPlugin.Instance.SomeField;
        }

        public override void Load()
        {
        }
    }
}
