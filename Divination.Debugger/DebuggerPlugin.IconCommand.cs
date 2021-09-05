using System;
using System.Linq;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Divination.Debugger
{
    public partial class DebuggerPlugin
    {
        [Command("/icon", Help = "FFXIV で使用可能なアイコンフォントの一覧を出力します。")]
        private void OnIconCommand()
        {
            Divination.Chat.Print(payloads =>
            {
                payloads.Add(new TextPayload("Icon fonts:\n"));

                foreach (var (i, icon) in Enum.GetValues(typeof(BitmapFontIcon)).Cast<BitmapFontIcon>().Select((x, i) => (i, x)))
                {
                    payloads.AddRange(new Payload[]
                    {
                        new TextPayload($"{i:D3}"),
                        new IconPayload(icon),
                        new TextPayload((i + 1) % 5 == 0 ? "\n" : " ")
                    });
                }
            });
        }
    }
}
