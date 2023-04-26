using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Divination.Common;

namespace Divination.ACT
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public abstract partial class DivinationActPlugin<TW, TU, TS>
    {
        private static readonly IDivinationLogger StatusLogger = DivinationLoggerFactory.Create("Status");
        private static readonly ConcurrentQueue<string> StatusTexts = new ConcurrentQueue<string>();

        public static void AppendStatusText(object message)
        {
            var line = $"[{DateTime.Now}] {message}";
            lock (StatusText)
            {
                if (StatusTexts.Count > 4)
                {
                    StatusTexts.TryDequeue(out _);
                }

                StatusTexts.Enqueue(line);

                StatusText.Text = string.Join(Environment.NewLine, StatusTexts.Reverse());
            }

            StatusLogger.Trace(line);
        }
    }
}
