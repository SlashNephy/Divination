using System.Threading.Tasks;
using System.Timers;
using DiscordRPC;

namespace Divination.DiscordIntegration
{
    public partial class DiscordIntegrationPlugin
    {
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
                    SmallImageText = "",
                },
            };

            discord.UpdatePresence(defaultPresence);
        }

        private void OnElapsed(object? sender, ElapsedEventArgs args)
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
    }
}
