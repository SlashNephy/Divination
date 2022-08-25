using System;

namespace Dalamud.Divination.Common.Api.Definition
{
    public interface IDefinitionManager<out TContainer> : IDisposable
        where TContainer : DefinitionContainer
    {
        public IDefinitionProvider<TContainer> Provider { get; }

        public TContainer Container { get; }

        public bool TryUpdate(string key, string? value, bool useTts);
    }
}
