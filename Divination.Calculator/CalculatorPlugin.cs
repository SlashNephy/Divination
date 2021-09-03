using System.Data;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;

namespace Divination.Calculator
{
    public class CalculatorPlugin : DivinationPlugin<CalculatorPlugin>, IDalamudPlugin, ICommandProvider
    {
        public CalculatorPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Logger.Information("Plugin loaded!");
        }

        [Command("/calc", "text", Help = "数式 <text> を計算し, 結果を表示します。", Strict = false)]
        private void OnCalcCommand(CommandContext context)
        {
            var result = new DataTable()
                .Compute(context.ArgumentText, null)
                .ToString() ?? string.Empty;

            Divination.Chat.Print(new SeString(
                new TextPayload($"{context.ArgumentText} = "),
                EmphasisItalicPayload.ItalicsOn,
                new TextPayload(result),
                EmphasisItalicPayload.ItalicsOff));
        }
    }
}
