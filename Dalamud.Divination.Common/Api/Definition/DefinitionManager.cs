using System.Collections.Generic;
using System.Reflection;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Utilities;

namespace Dalamud.Divination.Common.Api.Definition
{
    internal partial class DefinitionManager<TContainer> : IDefinitionManager<TContainer>, ICommandProvider where TContainer : DefinitionContainer, new()
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

        public bool TryUpdate(string key, string? value, bool useTts)
        {
            using var updater = new FieldUpdater(Provider.Container, chatClient, useTts);

            var fields = EnumerateDefinitionsFields();
            return updater.TryUpdate(key, value, fields);
        }

        public TContainer Container => Provider.Container;

        public void Dispose()
        {
            Provider.Dispose();
        }
    }
}
