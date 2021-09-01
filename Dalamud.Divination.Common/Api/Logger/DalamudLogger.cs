using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Dalamud.Divination.Common.Api.Logger
{
    internal sealed class DalamudLogger : IDisposable
    {
        private readonly object dalamud;

        private EventInfo? onLogLineEventInfo;
        private Delegate? onLogLineDelegate;
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(DalamudLogger));

        public DalamudLogger(object dalamud)
        {
            this.dalamud = dalamud;
        }

        private static ILogEventSink DalamudLogEventSink
        {
            get
            {
                var field = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .First(x => x.FullName == "Dalamud.Logging.Internal.SerilogEventSink")
                    .GetRuntimeProperty("Instance");

                // ReSharper disable once AssignNullToNotNullAttribute
                return (ILogEventSink) field!.GetValue(null)!;
            }
        }

        private LoggingLevelSwitch DalamudLoggingLevelSwitch
        {
            get
            {
                var property = dalamud.GetType()
                    .GetProperty("LogLevelSwitch", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

                // ReSharper disable once AssignNullToNotNullAttribute
                return (LoggingLevelSwitch) property!.GetValue(dalamud)!;
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void OnDalamudLogEvent(object sender, (string line, LogEventLevel level) args)
        {
            var line = args.line.Remove(0, args.line.IndexOf("] ", StringComparison.InvariantCulture) + 2);

            switch (args.level)
            {
                case LogEventLevel.Verbose:
                    logger.Verbose("{Line}", line);
                    return;
                case LogEventLevel.Debug:
                    logger.Debug("{Line}", line);
                    return;
                case LogEventLevel.Information:
                    logger.Information("{Line}", line);
                    return;
                case LogEventLevel.Warning:
                    logger.Warning("{Line}", line);
                    return;
                case LogEventLevel.Error:
                    logger.Error("{Line}", line);
                    return;
                case LogEventLevel.Fatal:
                    logger.Fatal("{Line}", line);
                    return;
            }
        }

        public void Subscribe()
        {
            try
            {
                onLogLineEventInfo = DalamudLogEventSink.GetType().GetEvent("OnLogLine");
                var method = GetType().BaseType!.GetMethod("OnDalamudLogEvent", BindingFlags.NonPublic | BindingFlags.Instance);
                onLogLineDelegate = Delegate.CreateDelegate(onLogLineEventInfo!.EventHandlerType!, this, method!);
                onLogLineEventInfo.AddEventHandler(DalamudLogEventSink, onLogLineDelegate);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Error occurred while SubscribeDalamudLog");
            }
        }

        private void Unsubscribe()
        {
            onLogLineEventInfo?.RemoveEventHandler(DalamudLogEventSink, onLogLineDelegate);
        }

        public void Dispose()
        {
            Unsubscribe();

            logger.Dispose();
        }
    }
}
