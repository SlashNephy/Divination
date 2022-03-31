using System.Diagnostics;
using Dalamud.Divination.Common.Api.Command.Attributes;

namespace Dalamud.Divination.Common.Api.Command
{
    public class DirectoryCommands : ICommandProvider
    {
        [Command("appdata")]
        [CommandHelp("Divination の AppData ディレクトリを開きます。")]
        [HiddenCommand(HideInHelp = false)]
        private static void OnAppDataCommand()
        {
            Process.Start(DivinationEnvironment.DivinationDirectory);
        }

        [Command("xlappdata")]
        [CommandHelp("XIVLauncher の AppData ディレクトリを開きます。")]
        [HiddenCommand(HideInHelp = false)]
        private static void OnXivLauncherAppDataCommand()
        {
            Process.Start(DivinationEnvironment.XivLauncherDirectory);
        }
    }
}
