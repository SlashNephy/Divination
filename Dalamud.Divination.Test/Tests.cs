using NUnit.Framework;

namespace Dalamud.Divination.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestPluginName()
        {
            var plugin = new TestPlugin();

            Assert.AreEqual("Divination.TestPlugin", plugin.Name);
        }

        [Test]
        public void TestPluginConfig()
        {
            var config = new TestConfig();

            Assert.AreEqual(1, config.Version);
        }

        [Test]
        public void TestReadField()
        {
            var plugin1 = new TestPlugin();
            var plugin2 = new AnotherPlugin();

            Assert.AreEqual("Hello, World!", AnotherPlugin.ReadField());
        }
    }
}
