using System;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;

namespace Divination.Template
{
    public class PowerUtilsPlugin : DivinationPlugin<PowerUtilsPlugin>, IDalamudPlugin, ICommandSupport
    {
        public PowerUtilsPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
        }

        public string MainCommandPrefix => "/power";

        [Command("/MonitorOff", "<seconds?>")]
        [CommandHelp("<seconds?> 秒後にディスプレイの電源を切ります。引数が指定されない場合は 5 秒後に電源を切ります。")]
        private void OnMonitorOffCommand(CommandContext context)
        {
            Task.Run(() =>
            {
                var arg = context["seconds"];
                var seconds = int.TryParse(arg, out var value) ? value : 5;

                Divination.Chat.Print($"{seconds.ToString()}秒後にディスプレイの電源がオフになります。");

                Thread.Sleep(seconds * 1000);
                Win32Api.SendMessage(-1, Win32Api.WM_SYSCOMMAND, Win32Api.SC_MONITORPOWER, Win32Api.MONITOR_OFF);
            });
        }

        [Command("save")]
        [CommandHelp("電源プランを「省電力」に設定します。")]
        private void OnPowerSaveCommand()
        {
            var plan = Guid.Parse("A1841308-3541-4FAB-BC81-F71556F20B4A");
            Win32Api.PowerSetActiveScheme(IntPtr.Zero, plan);

            Divination.Chat.Print("電源プランを「省電力」に設定しました。");
        }

        [Command("balance")]
        [CommandHelp("電源プランを「バランス」に設定します。")]
        private void OnPowerBalanceCommand()
        {
            var plan = Guid.Parse("381B4222-F694-41F0-9685-FF5BB260DF2E");
            Win32Api.PowerSetActiveScheme(IntPtr.Zero, plan);

            Divination.Chat.Print("電源プランを「バランス」に設定しました。");
        }

        [Command("perf")]
        [CommandHelp("電源プランを「高パフォーマンス」に設定します。")]
        private void OnPowerPerformanceCommand()
        {
            var plan = Guid.Parse("8C5E7FDA-E8BF-4A96-9A85-A6E23A8C635C");
            Win32Api.PowerSetActiveScheme(IntPtr.Zero, plan);

            Divination.Chat.Print("電源プランを「高パフォーマンス」に設定しました。");
        }
    }
}
