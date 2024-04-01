using Dalamud.Divination.Common.Api.Dalamud;
using DiscordRPC;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace Divination.DiscordIntegration.Discord;

public sealed class DiscordApi
{
    private const string ClientId = "745518092263620658";

    private DiscordRpcClient? rpcClient;

    private bool CreateRpcClient()
    {
        if (rpcClient?.IsDisposed == false)
        {
            return true;
        }

        rpcClient = new DiscordRpcClient(ClientId)
        {
            Logger = new DiscordRpcLogger(LogLevel.Info),
            SkipIdenticalPresence = true,
        };
        rpcClient.OnPresenceUpdate += (_, args) =>
        {
            DalamudLog.Log.Verbose(
                "RichPresence:\nDetails        = {Details}\nState          = {State}\nSmallImageText = {SmallImageText}\nLargeImageText = {LargeImageText}",
                args.Presence.Details,
                args.Presence.State,
                args.Presence.Assets.SmallImageText,
                args.Presence.Assets.LargeImageText);
        };
        rpcClient.OnConnectionFailed += (_, _) =>
        {
            rpcClient.Dispose();
        };

        return rpcClient.Initialize();
    }

    public void UpdatePresence(RichPresence presence)
    {
        if (!CreateRpcClient())
        {
            return;
        }

        rpcClient?.SetPresence(presence);
        if (rpcClient?.AutoEvents == false)
        {
            rpcClient?.Invoke();
        }
    }
}
