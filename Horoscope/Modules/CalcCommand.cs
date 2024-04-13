using System.Data;
using Dalamud.Game.Command;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Divination.Horoscope.Modules;

public class CalcCommand : IModule
{
    public string Name => "/calc Command";
    public string Description => "Compute expression and display the result. Support basic four arithmetic operations.";

    private const string CommandName = "/calc";

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
        var result = new DataTable().Compute(arguments, null).ToString() ?? string.Empty;

        Horoscope.Instance.Divination.Chat.Print(new SeString(
            new TextPayload($"{arguments} = "),
            EmphasisItalicPayload.ItalicsOn,
            new TextPayload(result),
            EmphasisItalicPayload.ItalicsOff));
    }
}
