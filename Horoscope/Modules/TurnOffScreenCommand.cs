using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dalamud.Game.Command;
using Divination.Horoscope.Modules;

namespace Divination.Horoscope;

public partial class TurnOffScreenCommand : IModule
{
    public string Name => "/turnoff Command";
    public string Description => "Turn off screen.";

    private const string CommandName = "/turnoff";

    public void Enable()
    {
        Horoscope.Instance.Dalamud.CommandManager.AddHandler(CommandName, new CommandInfo(OnDispatched)
        {
            HelpMessage = Description,
        });
    }

    public void Disable()
    {
        Horoscope.Instance.Dalamud.CommandManager.RemoveHandler(CommandName);
    }

    private void OnDispatched(string _, string arguments)
    {
        Task.Run(async () =>
        {
            var seconds = int.TryParse(arguments, out var value) ? value : 5;

            Horoscope.Instance.Divination.Chat.Print($"{seconds}秒後にディスプレイの電源がオフになります。");

            await Task.Delay(seconds * 1000);
            Win32Api.SendMessage(-1, Win32Api.WM_SYSCOMMAND, Win32Api.SC_MONITORPOWER, Win32Api.MONITOR_OFF);
        });
    }

    private static partial class Win32Api
    {
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MONITORPOWER = 0xF170;
        public const int MONITOR_OFF = 2;

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SendMessage(int hWnd, int msg, int wParam, int lParam);
    }
}
