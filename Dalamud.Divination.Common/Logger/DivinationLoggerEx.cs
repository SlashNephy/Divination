using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dalamud.Divination.Common.Logger
{
    public static class DivinationLoggerEx
    {
        private static IEnumerable<string> ToStringFuzzy(IEnumerable<object?> objects)
        {
            return objects.Select(o =>
            {
                return o switch
                {
                    string text => text,
                    IEnumerable enumerable => string.Join(", ", ToStringFuzzy(enumerable.Cast<object?>())),
                    _ => o?.ToString() ?? "null"
                };
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendEx(this IDivinationLogger logger, LogLevel level, params object?[] messages)
        {
            if (!logger.IsEnabledFor(level))
            {
                return;
            }

            var line = $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}] [{Enum.GetName(typeof(LogLevel), level),-5}] [{logger.Name}] {string.Join("\n", ToStringFuzzy(messages))}";
            logger.Append(LogLevel.Error, line);
        }

        public static void Error(this IDivinationLogger logger, params object?[] messages)
        {
            logger.AppendEx(LogLevel.Error, messages);
        }

        public static void Warn(this IDivinationLogger logger, params object?[] messages)
        {
            logger.AppendEx(LogLevel.Warn, messages);
        }

        public static void Info(this IDivinationLogger logger, params object?[] messages)
        {
            logger.AppendEx(LogLevel.Info, messages);
        }

        public static void Debug(this IDivinationLogger logger, params object?[] messages)
        {
            logger.AppendEx(LogLevel.Debug, messages);
        }

        public static void Trace(this IDivinationLogger logger, params object?[] messages)
        {
            logger.AppendEx(LogLevel.Trace, messages);
        }

        private static void AppendException(this IDivinationLogger logger, LogLevel level, Exception e, params object?[] messages)
        {
            var parameters = messages.Append(e.ToString());
            logger.AppendEx(level, parameters.ToArray());
        }

        public static void Error(this IDivinationLogger logger, Exception e, params object?[] messages)
        {
            logger.AppendException(LogLevel.Error, e, messages);
        }

        public static void Warn(this IDivinationLogger logger, Exception e, params object?[] messages)
        {
            logger.AppendException(LogLevel.Warn, e, messages);
        }
    }
}
