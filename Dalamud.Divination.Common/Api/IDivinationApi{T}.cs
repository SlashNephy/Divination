using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Divination.Common.Api.Ui.Window;

namespace Dalamud.Divination.Common.Api
{
    public interface IDivinationApi<TConfiguration, out TDefinition> : IDivinationApi
        where TConfiguration : class, IPluginConfiguration, new()
        where TDefinition : DefinitionContainer, new()
    {
        public IConfigManager<TConfiguration> Config { get; }

        public ConfigWindow<TConfiguration>? ConfigWindow { get; }

        public IDefinitionManager<TDefinition>? Definition { get; }
    }
}
