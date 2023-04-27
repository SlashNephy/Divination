using System.Data;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;

namespace Divination.Calculator;

public class Calculator : DivinationPlugin<Calculator>, IDalamudPlugin, ICommandProvider
{
    public Calculator(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
    }

    [Command("/calc", "<expression...>")]
    [CommandHelp("Compute <expression... > and display the result. Support basic four arithmetic operations.")]
    private void OnCalcCommand(CommandContext context)
    {
        var result = new DataTable()
            .Compute(context["text"], null)
            .ToString() ?? string.Empty;

        Divination.Chat.Print(new SeString(
            new TextPayload($"{context["text"]} = "),
            EmphasisItalicPayload.ItalicsOn,
            new TextPayload(result),
            EmphasisItalicPayload.ItalicsOff)
        );
    }
}
