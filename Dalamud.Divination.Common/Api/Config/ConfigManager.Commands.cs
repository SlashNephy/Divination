using System.Collections.Generic;
using System.Linq;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Config
{
    internal partial class ConfigManager<TConfiguration>
    {
        public object GetCommandInstance()
        {
            return new Commands(this);
        }

        private class Commands
        {
            private readonly ConfigManager<TConfiguration> manager;

            public Commands(ConfigManager<TConfiguration> manager)
            {
                this.manager = manager;
            }

            [Command("Config Show", Help = "プラグインの設定を出力します。")]
            private void OnConfigShowCommand()
            {
                manager.chatClient.Print(payloads =>
                {
                    payloads.Add(new TextPayload("設定一覧:\n"));

                    foreach (var fieldInfo in EnumerateConfigFields())
                    {
                        var name = fieldInfo.Name;
                        var value = fieldInfo.GetValue(manager.Config);

                        payloads.Add(new TextPayload($"{manager.commandProcessor.Prefix} config {name} {value}\n"));
                    }
                });
            }

            [Command("Config", "key?", "value?", Help = "プラグインの設定 <key> を <value?> に変更できます。<key> が null の場合, 利用可能な設定名の一覧を出力します。")]
            private void OnConfigCommand(CommandContext context, string? key, string? value)
            {
                if (key == null)
                {
                    var configKeys = EnumerateConfigFields().Select(x => x.Name);

                    manager.chatClient.PrintError(new List<Payload>
                    {
                        new TextPayload("コマンドの構文が間違っています。"),
                        new TextPayload($"Usage: {context.Command.Usage}"),
                        new TextPayload("設定名は Config.cs で定義されているフィールド名です。大文字小文字を区別しません。"),
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
