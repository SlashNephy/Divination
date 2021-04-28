using System;

namespace Dalamud.Divination.Common.Api.Definition
{
    public interface IDefinitionProvider<out TContainer> : IDisposable where TContainer : DefinitionContainer, new()
    {
        public TContainer Container { get; }
        public bool IsOutDated { get; }
        public bool AllowOutDatedDefinitions { get; }

        public void Update();
    }
}
