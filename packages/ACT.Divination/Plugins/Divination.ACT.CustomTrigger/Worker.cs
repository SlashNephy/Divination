using System.Threading.Tasks;
using Advanced_Combat_Tracker;
using Divination.Common;

#nullable enable
namespace Divination.ACT.CustomTrigger
{
    public class Worker : IPluginWorker
    {
        public Worker()
        {
            ActGlobals.oFormActMain.OnLogLineRead += OnNewLogLine;
        }

        public void Dispose()
        {
            ActGlobals.oFormActMain.OnLogLineRead -= OnNewLogLine;
        }

        private static void OnNewLogLine(bool isImport, LogLineEventArgs logInfo)
        {
            // Remove datetime prefix
            var line = logInfo.logLine.Remove(0, 15);

            foreach (var trigger in Plugin.Settings.Triggers)
            {
                Task.Run(() =>
                {
                    var match = trigger.Regex.Match(line);
                    if (!match.Success)
                    {
                        return;
                    }

                    var ev = new Trigger.Event(logInfo, match);
                    trigger.Action.Invoke(ev);

                    Plugin.Logger.Trace(line);
                });
            }
        }
    }
}
