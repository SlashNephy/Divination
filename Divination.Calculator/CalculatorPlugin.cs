using System.Data;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.Calculator
{
    public class CalculatorPlugin : DivinationPlugin<CalculatorPlugin>, IDalamudPlugin, ICommandProvider
    {
        public CalculatorPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            PluginLog.Information("Plugin loaded!");
        }

        [Command("/calc", "<text...>")]
        [CommandHelp("数式 <text...> を計算し, 結果を表示します。基本的な四則演算をサポートしています。")]
        private void OnCalcCommand(CommandContext context)
        {
            var result = new DataTable()
                .Compute(context["text"], null)
                .ToString() ?? string.Empty;

            Divination.Chat.Print(new SeString(
                new TextPayload($"{context["text"]} = "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(result),
                EmphasisItalicPayload.ItalicsOff));
        }
    }
}
