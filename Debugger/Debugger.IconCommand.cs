using System;
using System.Linq;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Divination.Debugger;

public partial class Debugger
{
    [Command("/icon")]
    [CommandHelp("FFXIV で使用可能なアイコンフォントの一覧を出力します。")]
    private void OnIconCommand()
    {
        Divination.Chat.Print(payloads =>
        {
            payloads.Add(new TextPayload("BitmapFontIcon:\n"));

            foreach (var (i, icon) in Enum.GetValues(typeof(BitmapFontIcon)).Cast<BitmapFontIcon>().Select((x, i) => (i, x)))
            {
                payloads.AddRange(new Payload[]
                {
                    new TextPayload($"{i:D3}"),
                    new IconPayload(icon),
                    new TextPayload((i + 1) % 5 == 0 ? "\n" : " "),
                });
            }

            payloads.Add(new TextPayload("\nSeIconChar:\n"));

            foreach (var (i, icon) in Enum.GetValues(typeof(SeIconChar)).Cast<SeIconChar>().Select((x, i) => (i, x)))
            {
                payloads.AddRange(new Payload[]
                {
                    new TextPayload($"{i:D3}"),
                    new TextPayload($"{(char)icon}"),
                    new TextPayload((i + 1) % 5 == 0 ? "\n" : " "),
                });
            }
        });
    }
}
