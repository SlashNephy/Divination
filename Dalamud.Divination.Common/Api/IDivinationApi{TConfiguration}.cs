using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Config;

namespace Dalamud.Divination.Common.Api
{
    public interface IDivinationApi<out TConfiguration> where TConfiguration : class, IPluginConfiguration, new()
    {
        public IConfigManager<TConfiguration> ConfigManager { get; }
    }
}
