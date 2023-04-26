using Dalamud.Configuration;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate;

public abstract class
    DivinationPlugin<TPlugin, TConfiguration> : DivinationPlugin<TPlugin, TConfiguration, EmptyDefinitionContainer>
    where TPlugin : DivinationPlugin<TPlugin, TConfiguration, EmptyDefinitionContainer>
    where TConfiguration : class, IPluginConfiguration, new()
{
    protected DivinationPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
    }
}
