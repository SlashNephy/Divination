using System.IO;
using System.Threading;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Dalamud.Divination.Common.Api.Logger
{
    /// <summary>
    /// Divination プラグインで使用されるロガーを生成する静的クラスです。
    /// </summary>
    public static class DivinationLogger
    {
        /// <summary>
        /// 指定した名前のロガーを生成します。
        /// </summary>
        /// <param name="name">ロガーの名前。ログファイルの名前になります。</param>
        /// <returns>Serilog.Core.Logger</returns>
        public static Serilog.Core.Logger Of(string name)
        {
            return new LoggerConfiguration()
#if DEBUG
                .Enrich.With(new ThreadNameEnricher())
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:yyyy/MM/dd HH:mm:ss.fff}] [{Level}] [{Thread}] {Message}{NewLine}{Exception}")
#endif
                .WriteTo.File(
                    Path.Combine(DivinationEnvironment.LogDirectory, $"{name}.log"),
                    LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

#if DEBUG
#pragma warning disable 162
            ConsoleWindow.Show();
#pragma warning restore 162
#endif
        }

        private class ThreadNameEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddPropertyIfAbsent(
                    propertyFactory.CreateProperty("Thread", Thread.CurrentThread.Name ?? $"Thread#{Thread.CurrentThread.ManagedThreadId.ToString()}")
                );
            }
        }
    }
}
