using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Utilities;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Dalamud.Divination.Common.Api.Definition
{
    public partial class DefinitionManager<TContainer> : IDefinitionManager<TContainer>, ICommandProvider where TContainer : DefinitionContainer, new()
    {
        public IDefinitionProvider<TContainer> Provider { get; }

        private readonly IChatClient chatClient;

        public DefinitionManager(string url, IChatClient chatClient)
        {
            Provider = DefinitionProviderFactory<TContainer>.Create(url);
            this.chatClient = chatClient;
        }

        private IEnumerable<FieldInfo> EnumerateDefinitionsFields()
        {
            return Provider.Container.GetType().GetFields();
        }

        public bool TryUpdate(string key, string? value)
        {
            var fieldInfo = EnumerateDefinitionsFields().FirstOrDefault(x => x.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (fieldInfo == null)
            {
                chatClient.PrintError(new List<Payload> {
                    new TextPayload("指定された設定名 "),
                    EmphasisItalicPayload.ItalicsOn,
                    new TextPayload(key),
                    EmphasisItalicPayload.ItalicsOff,
                    new TextPayload(" は存在しません。")
                });

                return false;
            }

            var fieldValue = fieldInfo.GetValue(Provider.Container);
            var updater = new FieldUpdater(Provider.Container, fieldInfo, chatClient);
            switch (fieldValue)
            {
                case byte:
                    return updater.UpdateByteField(value);
                case ushort:
                    return updater.UpdateUInt16Field(value);
                case int:
                    return updater.UpdateIntField(value);
                default:
                    chatClient.PrintError(new List<Payload> {
                        new TextPayload("指定された設定名 "),
                        EmphasisItalicPayload.ItalicsOn,
                        new TextPayload(key),
                        EmphasisItalicPayload.ItalicsOff,
                        new TextPayload(" の変更はサポートされていません。")
                    });

                    return false;
            }
        }

        public void Dispose()
        {
            Provider.Dispose();
        }
    }
}
