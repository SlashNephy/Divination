using System.Timers;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using Divination.DiscordIntegration.Discord;
using Divination.DiscordIntegration.Ipc;

namespace Divination.DiscordIntegration
{
    public partial class DiscordIntegrationPlugin : DivinationPlugin<DiscordIntegrationPlugin, PluginConfig, PluginDefinition>, IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>, IDefinitionSupport
    {
        private readonly DiscordApi discord = new();
        private readonly Timer timer = new(3000);

        public DiscordIntegrationPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            SetDefaultPresence();

            timer.Elapsed += OnElapsed;
            timer.Start();

            IpcManager.Register();
        }

        public string MainCommandPrefix => "/discord";
        public string DefinitionUrl => "https://horoscope-dev.github.io/Dalamud.Divination.Ephemera/dist/DiscordIntegration.json";

        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

        protected override void ReleaseManaged()
        {
            IpcManager.Unregister();
            timer.Dispose();
            discord.Dispose();
        }
    }
}
