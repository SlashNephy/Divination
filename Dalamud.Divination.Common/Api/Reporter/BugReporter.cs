using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Logging;
using Newtonsoft.Json;

namespace Dalamud.Divination.Common.Api.Reporter
{
    internal sealed partial class BugReporter : IBugReporter
    {
        private const string DefaultUrl = "https://divination.horoscope.dev/collect/bug_report";

        private readonly string pluginName;
        private readonly IVersionManager versionManager;
        private readonly IChatClient chat;
        private readonly string url;
        private readonly HttpClient httpClient = new();

        public BugReporter(string pluginName, IVersionManager versionManager, IChatClient chat, string url = DefaultUrl)
        {
            this.pluginName = pluginName;
            this.versionManager = versionManager;
            this.chat = chat;
            this.url = url;
        }

        private static string DalamudLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher", "dalamud.log");

        public async Task SendAsync(string message)
        {
            await SendReportAsync(DalamudLogPath, message);

            foreach (var path in Directory.GetFiles(DivinationEnvironment.LogDirectory, $"{pluginName}*.log"))
            {
                await SendReportAsync(path, message);

                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
            }
        }

        private async Task SendReportAsync(string path, string message)
        {
            if (!File.Exists(path))
            {
                return;
            }

            await using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(file, Encoding.UTF8);

            var filename = Path.GetFileName(path);
            var payload = new Dictionary<string, string>
            {
                {"plugin", pluginName},
                {"message", message},
                {"file", await reader.ReadToEndAsync()},
                {"filename", filename},
                {"version", versionManager.Plugin.InformationalVersion},
                {"library_version", versionManager.Divination.InformationalVersion}
            };

            var json = JsonConvert.SerializeObject(payload, Formatting.None);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                await httpClient.PostAsync(url, content);
                chat.Print($"レポート ({filename}) を送信しました。");
            }
            catch (Exception exception)
            {
                chat.PrintError($"レポート ({filename}) の送信に失敗しました。");
                PluginLog.Error(exception, "Error occurred while SendReport");
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
