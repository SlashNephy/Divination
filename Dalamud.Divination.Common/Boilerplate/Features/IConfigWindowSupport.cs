using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Ui.Window;

namespace Dalamud.Divination.Common.Boilerplate.Features
{
    public interface IConfigWindowSupport<TConfiguration> where TConfiguration : class, IPluginConfiguration, new()
    {
        public ConfigWindow<TConfiguration> CreateConfigWindow();
    }
}
