using System.Reflection;
using Dalamud.Divination.Common.Boilerplate;

namespace Dalamud.Divination.Test
{
    public class AnotherPlugin : DivinationPlugin<AnotherPlugin, TestConfig>
    {
        public override string Name => "AnotherPlugin";

        public override Assembly Assembly => Assembly.GetExecutingAssembly();

        public string ReadField()
        {
            return TestPlugin.Instance.SomeField;
        }

        public override void Load()
        {
            throw new System.NotImplementedException();
        }

        public override void Unload()
        {
            throw new System.NotImplementedException();
        }
    }
}
