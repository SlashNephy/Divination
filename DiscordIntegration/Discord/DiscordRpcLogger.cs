using Dalamud.Divination.Common.Api.Dalamud;
using DiscordRPC.Logging;

namespace Divination.DiscordIntegration.Discord;

public class DiscordRpcLogger(LogLevel level) : ILogger
{
    public LogLevel Level { get; set; } = level;

    public void Trace(string message, params object[] args)
    {
        if (Level > LogLevel.Trace)
        {
            return;
        }

        DalamudLog.Log.Verbose(message, args);
    }

    public void Info(string message, params object[] args)
    {
        if (Level > LogLevel.Info)
        {
            return;
        }

        DalamudLog.Log.Information(message, args);
    }

    public void Warning(string message, params object[] args)
    {
        if (Level > LogLevel.Warning)
        {
            return;
        }

        DalamudLog.Log.Warning(message, args);
    }

    public void Error(string message, params object[] args)
    {
        if (Level > LogLevel.Error)
        {
            return;
        }

        DalamudLog.Log.Error(message, args);
    }
}
