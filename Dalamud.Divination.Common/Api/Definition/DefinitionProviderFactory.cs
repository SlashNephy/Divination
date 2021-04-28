namespace Dalamud.Divination.Common.Api.Definition
{
    public static class DefinitionProviderFactory<TContainer> where TContainer : DefinitionContainer, new()
    {
        public static IDefinitionProvider<TContainer> Create()
        {
#if DEBUG
            return new LocalDefinitionProvider<TContainer>();
#else
            return new RemoteDefinitionProvider<TContainer>();
#endif
        }
    }
}
