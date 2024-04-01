using System.Timers;
using DiscordRPC;
using Divination.DiscordIntegration.Discord;

namespace Divination.DiscordIntegration;

public partial class DiscordIntegration
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

        DiscordRpc.UpdatePresence(defaultPresence);
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
    }

    private static void UpdatePresence()
    {
        var presence = Formatter.CreatePresence();
        if (presence != null)
        {
            DiscordRpc.UpdatePresence(presence);
        }
    }
}
