using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Dalamud.Payload;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Command
{
    internal sealed partial class CommandProcessor
    {
        public class DefaultCommands : ICommandProvider
        {
            private readonly CommandProcessor processor;

            public DefaultCommands(CommandProcessor processor)
            {
                this.processor = processor;
            }

            [Command("help")]
            [CommandHelp("プラグインのヘルプを表示します。")]
            private void OnHelpCommand()
            {
                processor.chatClient.Print(payloads =>
                {
                    payloads.Add(new TextPayload($"{processor.pluginName} のコマンド一覧:\n"));

                    foreach (var command in processor.Commands)
                    {
                        payloads.AddRange(PayloadUtilities.HighlightAngleBrackets(command.Usage));

                        if (!string.IsNullOrEmpty(command.Help))
                        {
                            payloads.Add(new TextPayload($"\n {SeIconChar.ArrowRight.AsString()} "));
                            payloads.AddRange(PayloadUtilities.HighlightAngleBrackets(command.Help));
                        }

                        payloads.Add(new TextPayload("\n"));
                    }
                });
            }
        }
    }
}
