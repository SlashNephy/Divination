using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract class DivinationPlugin<TPlugin> : DivinationPlugin<TPlugin, EmptyConfig, EmptyDefinitionContainer>
        where TPlugin : DivinationPlugin<TPlugin, EmptyConfig, EmptyDefinitionContainer>
    {
        protected DivinationPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
        }
    }
}
