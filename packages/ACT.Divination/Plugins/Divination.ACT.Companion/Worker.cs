using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Divination.Common;
using Newtonsoft.Json;

#nullable enable
namespace Divination.ACT.Companion
{
    public class Worker : IPluginWorker
    {
        private readonly Timer timer;

        public Worker()
        {
            StartProcesses();

            timer = new Timer(60 * 1000);
            timer.Elapsed += (sender, args) => StartProcesses();
            timer.Start();
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();

            KillProcesses();
        }

        private static void StartProcesses()
        {
            foreach (ProcessType type in Enum.GetValues(typeof(ProcessType)))
            {
                if (StartProcess(type))
                {
                    if (type == ProcessType.Voiceroid2Proxy && Settings.Voiceroid2ProxyTtsOnLoad)
                    {
                        Task.Run(async () =>
                        {
                            var payload = new Dictionary<string, string>
                            {
                                {"text", Settings.Voiceroid2ProxyStartupMessage}
                            };
                            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
                                "application/json");

                            await Plugin.HttpClient.PostAsync("http://localhost:4532/talk", content);
                        });
                    }
                }
            }
        }

        private static bool StartProcess(ProcessType type)
        {
            var path = Settings.ExecutablePath(type);
            if (path == null)
            {
                return false;
            }

            var imageName = Path.GetFileNameWithoutExtension(path);

            var process = Process.GetProcessesByName(imageName).FirstOrDefault();
            if (process == null)
            {
                process = new Process
                {
                    StartInfo =
                    {
                        FileName = path,
                        WorkingDirectory = Path.GetDirectoryName(path) ?? Plugin.AssemblyDirectory,
                        WindowStyle = ProcessWindowStyle.Minimized
                    }
                };
                process.Start();

                Plugin.Logger.Debug($"{imageName}.exe started!");
                return true;
            }

            return false;
        }

        private static void KillProcesses()
        {
            foreach (ProcessType type in Enum.GetValues(typeof(ProcessType)))
            {
                KillProcess(type);
            }
        }

        private static void KillProcess(ProcessType type)
        {
            if (!Settings.ExitOnUnload(type))
            {
                return;
            }

            var path = Settings.ExecutablePath(type);
            if (path == null)
            {
                return;
            }

            var imageName = Path.GetFileNameWithoutExtension(path);

            var process = Process.GetProcessesByName(imageName).FirstOrDefault();
            if (process != null)
            {
                process.Kill();

                Plugin.Logger.Debug($"{process.ProcessName}.exe stopped!");
            }
        }
    }
}
