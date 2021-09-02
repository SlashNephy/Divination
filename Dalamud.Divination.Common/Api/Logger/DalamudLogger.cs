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
        private EventInfo? onLogLineEventInfo;
        private Delegate? onLogLineDelegate;
        private readonly Serilog.Core.Logger logger = DivinationLogger.Debug(nameof(DalamudLogger));

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

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void OnDalamudLogEvent(object sender, (string Line, LogEventLevel Level, DateTimeOffset TimeStamp) args)
        {
            var line = args.Line.Remove(0, args.Line.IndexOf("] ", StringComparison.InvariantCulture) + 2);

            switch (args.Level)
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
                var method = GetType().GetMethod("OnDalamudLogEvent", BindingFlags.NonPublic | BindingFlags.Instance);
                onLogLineDelegate = Delegate.CreateDelegate(onLogLineEventInfo!.EventHandlerType!, this, method!);
                onLogLineEventInfo.AddEventHandler(DalamudLogEventSink, onLogLineDelegate);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Error occurred while Subscribe");
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
