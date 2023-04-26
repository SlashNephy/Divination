using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Definition;

internal partial class DefinitionManager<TContainer>
{
    public class Commands : ICommandProvider
    {
        private readonly DefinitionManager<TContainer> manager;

        public Commands(IDefinitionManager<TContainer> manager)
        {
            this.manager = (DefinitionManager<TContainer>)manager;
        }

        [Command("def", "version")]
        [CommandHelp("{Name} が参照している、定義ファイルのバージョンを表示します。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnDefVersionCommand()
        {
            manager.chatClient.Print(
                $"定義ファイル: ゲームバージョン = {manager.Provider.Container.Version}, パッチ = {manager.Provider.Container.Patch}");
        }

        [Command("def", "fetch")]
        [CommandHelp("{Name} の定義ファイルを再取得します。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnDefFetchCommand()
        {
            manager.Provider.Update(CancellationToken.None);
            manager.chatClient.Print(
                $"定義ファイルを再取得しました。ゲームバージョン = {manager.Provider.Container.Version}, パッチ = {manager.Provider.Container.Patch}");
        }

        [Command("def", "show")]
        [CommandHelp("{Name} の現在の定義を出力します。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnDefShowCommand()
        {
            manager.chatClient.Print(payloads =>
            {
                payloads.Add(new TextPayload("定義一覧:\n"));

                foreach (var fieldInfo in manager.EnumerateDefinitionsFields())
                {
                    var name = fieldInfo.Name;
                    var value = fieldInfo.GetValue(manager.Container);

                    payloads.Add(new TextPayload($"{name} = {value}\n"));
                }
            });
        }

        [Command("def")]
        [CommandHelp("{Name} で利用可能な定義名の一覧を出力します。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnDefListCommand()
        {
            var defKeys = manager.EnumerateDefinitionsFields().Select(x => x.Name);

            manager.chatClient.Print(new List<Payload>
            {
                new TextPayload($"定義名は {typeof(TContainer).FullName} で定義されているフィールド名です。大文字小文字を区別しません。\n"),
                new TextPayload("定義値が空白の場合, null として設定します。\n"),
                new TextPayload("利用可能な定義名の一覧:\n"),
                new TextPayload(string.Join("\n", defKeys)),
            });
        }

        [Command("def", "<key>", "<value>")]
        [Command("deftts", "<key>", "<value>")]
        [CommandHelp("{Name} の定義 <key> を <value> に上書きします。")]
        [HiddenCommand(HideInHelp = false)]
        private void OnDefUpdateCommand(CommandContext context)
        {
            var key = context.GetArgument("key");
            var value = context.GetArgument("value");

            manager.TryUpdate(key, value, context.Command.Syntaxes[1] == "deftts");
        }
    }
}
