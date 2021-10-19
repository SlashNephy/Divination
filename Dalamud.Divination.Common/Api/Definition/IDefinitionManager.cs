using System;

namespace Dalamud.Divination.Common.Api.Definition
{
    public interface IDefinitionManager<out TContainer> : IDisposable where TContainer : DefinitionContainer, new()
    {
        public IDefinitionProvider<TContainer> Provider { get; }

        public bool TryUpdate(string key, string? value, bool useTts);

        public TContainer Container { get; }
    }
}
