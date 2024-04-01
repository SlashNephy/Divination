using DiscordRPC;
using DiscordRPC.Message;
using LogLevel = DiscordRPC.Logging.LogLevel;

namespace Divination.DiscordIntegration.Discord;

public static class DiscordRpc
{
    private const string ApplicationId = "1224354099877773323";

    private static DiscordRpcClient? _client;

    public static PresenceMessage? LastPresence { get; private set; }

    private static bool InitializeClient()
    {
        if (_client?.IsDisposed == false)
        {
            return true;
        }

        _client = new DiscordRpcClient(ApplicationId)
        {
            Logger = new DiscordRpcLogger(LogLevel.Info),
            SkipIdenticalPresence = true,
        };
        _client.OnPresenceUpdate += (_, args) =>
        {
            LastPresence = args;
        };
        _client.OnConnectionFailed += (_, _) =>
        {
            _client.Dispose();
        };

        return _client.Initialize();
    }

    public static void UpdatePresence(RichPresence presence)
    {
        if (!InitializeClient())
        {
            return;
        }

        _client?.SetPresence(presence);
        if (_client?.AutoEvents == false)
        {
            _client?.Invoke();
        }
    }
}
