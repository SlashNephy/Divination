using System.Threading.Tasks;
using System.Timers;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using DiscordRPC;
using Divination.DiscordIntegration.Discord;

namespace Divination.DiscordIntegration
{
    public class DiscordIntegrationPlugin : DivinationPlugin<DiscordIntegrationPlugin, PluginConfig, PluginDefinition>, IDalamudPlugin, ICommandSupport, IConfigWindowSupport<PluginConfig>, IDefinitionSupport
    {
        private readonly DiscordApi discord = new();
        private readonly Timer timer = new(3000);

        public DiscordIntegrationPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            SetDefaultPresence();

            timer.Elapsed += OnElapsed;
            timer.Start();
        }

        public string MainCommandPrefix => "/discord";
        public string DefinitionUrl => "https://raw.githubusercontent.com/SlashNephy/Dalamud.Divination.Ephemera/master/dist/DiscordIntegration.json";

        public ConfigWindow<PluginConfig> CreateConfigWindow() => new PluginConfigWindow();

        private void SetDefaultPresence()
        {
            var defaultPresence = new RichPresence
            {
                Details = "メニュー",
                State = "",
                Assets = new Assets
                {
                    LargeImageKey = "li_1",
                    LargeImageText = "",
                    SmallImageKey = "class_0",
                    SmallImageText = ""
                }
            };

            discord.UpdatePresence(defaultPresence);
        }

        private void OnElapsed(object sender, ElapsedEventArgs args)
        {
            Update();
        }

        private void Update()
        {
            if (!Dalamud.ClientState.IsLoggedIn)
            {
                return;
            }

            Formatter.Reset();

            UpdatePresence();
            UpdateCustomStatus();
        }

        private void UpdatePresence()
        {
            var presence = Formatter.CreatePresence();
            if (presence != null)
            {
                discord.UpdatePresence(presence);
            }
        }

        private void UpdateCustomStatus()
        {
            if (Config.ShowCustomStatus)
            {
                var status = Formatter.CreateCustomStatus();
                if (status != null)
                {
                    Task.Run(async () =>
                    {
                        var (a, b, c) = status.Value;
                        await discord.UpdateCustomStatus(a, b, c);
                    });
                }
            }
        }

        protected override void ReleaseManaged()
        {
            timer.Dispose();
            discord.Dispose();
        }
    }
}
