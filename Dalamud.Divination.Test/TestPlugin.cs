using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;

namespace Dalamud.Divination.Test
{
    public class TestPlugin : DivinationPlugin<TestPlugin, TestConfig>
    {
        public readonly string SomeField = "Hello, World!";

        public override string Name => "Divination.TestPlugin";
        public override Assembly Assembly => Assembly.GetExecutingAssembly();

        public override void Load()
        {
            Logger.Information("Loaded!");
        }

        public override void Unload()
        {
            Logger.Information("Unloaded!");
        }
    }
}
