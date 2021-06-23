using System;
using System.Diagnostics;
using System.IO;

namespace Dalamud.Divination.Common.Api.Command
{
    public class DirectoryCommands : ICommandProvider
    {
        [Command("AppData", Help = "Divination の AppData ディレクトリを開きます。")]
        private static void OnAppDataCommand()
        {
            Process.Start(DivinationEnvironment.DivinationDirectory);
        }

        [Command("XLAppData", Help = "XIVLauncher の AppData ディレクトリを開きます。")]
        private static void OnXivLauncherAppDataCommand()
        {
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher"));
        }
    }
}
