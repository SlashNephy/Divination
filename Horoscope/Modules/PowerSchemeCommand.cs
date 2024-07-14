using System;
using System.Runtime.InteropServices;
using Dalamud.Game.Command;
using Divination.Horoscope.Modules;

namespace Divination.Horoscope;

public class PowerSchemeCommand : IModule
{
    public string Id => "power_scheme_command";
    public string Name => "/power Command";
    public string Description => "Set active Windows power scheme.";

    private const string CommandName = "/power";

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

    private readonly Guid powerSaverSchemeId = Guid.Parse("a1841308-3541-4fab-bc81-f71556f20b4a");
    private readonly Guid balancedSchemeId = Guid.Parse("381b4222-f694-41f0-9685-ff5bb260df2e");
    private readonly Guid highPerformanceSchemeId = Guid.Parse("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");

    private void OnDispatched(string _, string arguments)
    {
        switch (arguments)
        {
            case "power_saver":
            case "save":
            case "saving":
            case "saver":
            case "s":
                SetActivePowerScheme(powerSaverSchemeId, "省電力");
                break;
            case "balanced":
            case "balance":
            case "b":
                SetActivePowerScheme(balancedSchemeId, "バランス");
                break;
            case "high_performance":
            case "performance":
            case "perf":
            case "p":
                SetActivePowerScheme(highPerformanceSchemeId, "高パフォーマンス");
                break;
            default:
                Horoscope.Instance.Divination.Chat.PrintError("引数が正しくありません。");
                break;
        }
    }

    private void SetActivePowerScheme(Guid schemeId, string name)
    {
        Win32Api.PowerSetActiveScheme(IntPtr.Zero, schemeId);
        Horoscope.Instance.Divination.Chat.Print($"電源プランを「{name}」に設定しました。");
    }

    private static class Win32Api
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Unicode)]
        public static extern uint PowerSetActiveScheme(IntPtr rootPowerKey, [MarshalAs(UnmanagedType.LPStruct)] Guid schemeGuid);
    }
}
