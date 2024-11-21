using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Lumina.Excel.Sheets;

namespace Divination.Debugger;

public partial class Debugger
{
    [Command("/color")]
    [CommandHelp("FFXIV で使用可能なカラーコードの一覧を出力します。")]
    private void OnColorCommand()
    {
        Divination.Chat.Print(payloads =>
        {
            payloads.Add(new TextPayload("UIForeground:\n"));

            var i = 0;
            foreach (var color in Dalamud.DataManager.GetExcelSheet<UIColor>()!)
            {
                payloads.AddRange(new Payload[]
                {
                    new UIForegroundPayload((ushort)color.RowId),
                    new TextPayload($"{color.RowId:D3}"),
                    UIForegroundPayload.UIForegroundOff,
                    new TextPayload(++i % 10 == 0 ? "\n" : " "),
                });
            }

            payloads.Add(new TextPayload("\nUIGlow:\n"));

            i = 0;
            foreach (var color in Dalamud.DataManager.GetExcelSheet<UIColor>()!)
            {
                payloads.AddRange(new Payload[]
                {
                    new UIGlowPayload((ushort)color.RowId),
                    new TextPayload($"{color.RowId:D3}"),
                    UIGlowPayload.UIGlowOff,
                    new TextPayload(++i % 10 == 0 ? "\n" : " "),
                });
            }
        });
    }
}
