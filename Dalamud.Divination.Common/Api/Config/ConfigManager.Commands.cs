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
            [CommandHelp("プラグインの設定を出力します。")]
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
            [Command("config", "<key?>")]
            [Command("config", "<key?>", "<value?>")]
            [CommandHelp("プラグインの設定 <key?> を <value?> に変更できます。<key?> が null の場合, 利用可能な設定名の一覧を出力します。")]
            private void OnConfigCommand(CommandContext context)
            {
                var key = context["key"];
                var value = context["value"];
                if (key == null)
                {
                    var configKeys = EnumerateConfigFields().Select(x => x.Name);

                    chatClient.PrintError(new List<Payload>
                    {
                        new TextPayload("コマンドの構文が間違っています。"),
                        new TextPayload($"Usage: {context.Command.Usage}"),
                        new TextPayload($"設定名は {typeof(TConfiguration).FullName} で定義されているフィールド名です。大文字小文字を区別しません。"),
                        new TextPayload("設定値が bool/string の場合, 設定値を省略することができます。bool の場合はトグルされ, string の場合は空白値として設定します。"),
                        new TextPayload("利用可能な設定名の一覧:"),
                        new TextPayload(string.Join("\n", configKeys))
                    });

                    return;
                }

                manager.TryUpdate(key, value);
            }
        }
    }
}
