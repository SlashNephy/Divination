using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Divination.ACT.DiscordIntegration.Properties;
using Divination.Common;
using Sharlayan;
using Sharlayan.Models;
using Language = FFXIV_ACT_Plugin.Common.Language;

#nullable enable
namespace Divination.ACT.DiscordIntegration
{
    public class Worker : IPluginWorker
    {
        private readonly Timer timer;

        private Process? attachedProcess;

        public Worker()
        {
            DiscordApi.Release();

            Check();

            timer = new Timer(Settings.CheckIntervalMs);
            timer.Elapsed += (source, args) => Check();
            timer.Start();
        }

        public void Dispose()
        {
            timer.Dispose();
            MemoryHandler.Instance.UnsetProcess();

            DiscordApi.Release();
        }

        private void Check()
        {
            try
            {
                if (AttachToGameProcess())
                {
                    var process = Plugin.DataRepository.GetCurrentFFXIVProcess();
                    Plugin.TabControl.sharlayanLabel.Text = Resources.ProcessAttached;
                    Plugin.TabControl.processLabel.Text = string.Format(Resources.ProcessName,
                        process?.ProcessName, process?.Id);

                    PresenceUpdater.AsGame();
                    return;
                }

                Plugin.TabControl.sharlayanLabel.Text = Resources.ProcessNotAttached;

                var launcherProcess = Process.GetProcessesByName("ffxivlauncher").FirstOrDefault() ??
                                      Process.GetProcessesByName("ffxivlauncher64").FirstOrDefault();
                if (launcherProcess != null)
                {
                    Plugin.TabControl.processLabel.Text = string.Format(Resources.ProcessName,
                        launcherProcess.ProcessName, launcherProcess.Id);

                    PresenceUpdater.AsLauncher();
                    return;
                }

                Plugin.TabControl.processLabel.Text = Resources.ProcessNotFound;
                DiscordApi.Release();
            }
            catch (Exception e)
            {
                Plugin.Logger.Error(e);
            }
        }

        private bool AttachToGameProcess()
        {
            var process = Plugin.DataRepository.GetCurrentFFXIVProcess();
            if (process == null)
            {
                MemoryHandler.Instance.UnsetProcess();

                return false;
            }

            if (process == attachedProcess && MemoryHandler.Instance.IsAttached)
            {
                return true;
            }

            var processModel = new ProcessModel
            {
                Process = process,
                IsWin64 = true
            };

            MemoryHandler.Instance.SetProcess(processModel,
                Enum.GetName(typeof(Language), Plugin.DataRepository.GetSelectedLanguageID()));
            attachedProcess = process;

            return true;
        }
    }
}
