using Dalamud.Divination.Common.Api.Dalamud;
using DiscordRPC.Logging;

namespace Divination.DiscordIntegration.Discord;

public class DiscordRpcLogger : ILogger
{
    public LogLevel Level { get; set; }

    public DiscordRpcLogger(LogLevel level)
    {
        Level = level;
    }

    public void Trace(string message, params object[] args)
    {
        if (Level > LogLevel.Trace)
        {
            return;
        }

#pragma warning disable CA1416
        DalamudLog.Log.Verbose(message, args);
#pragma warning restore CA1416
    }

    public void Info(string message, params object[] args)
    {
        if (Level > LogLevel.Info)
        {
            return;
        }

#pragma warning disable CA1416
        DalamudLog.Log.Information(message, args);
#pragma warning restore CA1416
    }

    public void Warning(string message, params object[] args)
    {
        if (Level > LogLevel.Warning)
        {
            return;
        }

#pragma warning disable CA1416
        DalamudLog.Log.Warning(message, args);
#pragma warning restore CA1416
    }

    public void Error(string message, params object[] args)
    {
        if (Level > LogLevel.Error)
        {
            return;
        }

#pragma warning disable CA1416
        DalamudLog.Log.Error(message, args);
#pragma warning restore CA1416
    }
}
