using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Config
{
    internal partial class ConfigManager<TConfiguration>
    {
        public class Commands : ICommandProvider
        {
            private readonly IConfigManager<TConfiguration> manager;
            private readonly ICommandProcessor processor;
            private readonly IChatClient chatClient;

            public Commands(IConfigManager<TConfiguration> manager, ICommandProcessor processor, IChatClient chatClient)
            {
                this.manager = manager;
                this.processor = processor;
                this.chatClient = chatClient;
            }

            [Command("config", "show")]
            [CommandHelp("{Name} の現在の設定値を出力します。")]
            [HiddenCommand(HideInHelp = false)]
            private void OnConfigShowCommand()
            {
                chatClient.Print(payloads =>
                {
                    payloads.Add(new TextPayload("設定一覧:\n"));

                    foreach (var fieldInfo in EnumerateConfigFields())
                    {
                        var name = fieldInfo.Name;
                        var value = fieldInfo.GetValue(manager.Config);

                        payloads.Add(new TextPayload($"{processor.Prefix} config {name} {value}\n"));
                    }
                });
            }

            [Command("config")]
            [CommandHelp("{Name} で利用可能な設定名の一覧を出力します。")]
            [HiddenCommand(HideInHelp = false)]
            private void OnConfigListCommand(CommandContext context)
            {
                var configKeys = EnumerateConfigFields().Select(x => x.Name);

                chatClient.Print(new List<Payload>
                {
                    new TextPayload($"設定名は {typeof(TConfiguration).FullName} で定義されているフィールド名です。大文字小文字を区別しません。\n"),
                    new TextPayload("設定値が bool/string の場合, 設定値を省略することができます。bool の場合はトグルされ, string の場合は空白値として設定します。\n"),
                    new TextPayload("利用可能な設定名の一覧:\n"),
                    new TextPayload(string.Join("\n", configKeys))
                });
            }

            [Command("config", "<key>", "<value?>")]
            [Command("configtts", "<key>", "<value?>")]
            [CommandHelp("{Name} の設定 <key> を <value?> に変更できます。")]
            [HiddenCommand(HideInHelp = false)]
            private void OnConfigUpdateCommand(CommandContext context)
            {
                var key = context.GetArgument("key");
                var value = context["value"];

                manager.TryUpdate(key, value, context.Command.Syntaxes[1] == "configtts");
            }
        }
    }
}
