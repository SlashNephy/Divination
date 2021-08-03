using System.Text.RegularExpressions;
using Advanced_Combat_Tracker;
using Divination.ACT.MobKillsCounter.View;

#nullable enable
namespace Divination.ACT.MobKillsCounter
{
    public class Worker : IPluginWorker
    {
        public Worker()
        {
            CounterView = new CounterView();

            ActGlobals.oFormActMain.OnLogLineRead += OnLogLineRead;
        }

        public CounterView CounterView { get; }

        public void Dispose()
        {
            ActGlobals.oFormActMain.OnLogLineRead -= OnLogLineRead;

            CounterView.Close();
        }

        private void OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            var logLine = logInfo.logLine.Remove(0, 15);

            if (!logLine.StartsWith("19:"))
            {
                return;
            }

            const string logPattern = @"^19:(.+?) was defeated by (.+)\.$";
            var match = Regex.Match(logLine, logPattern);

            if (!match.Success)
            {
                return;
            }

            if (!Plugin.TabControl.IsAllKillsEnable() && match.Groups[2].Value != ActGlobals.charName)
            {
                return;
            }

            CounterView.CountUp(match.Groups[1].Value);
        }
    }
}
