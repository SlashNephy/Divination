using Dalamud.Configuration;

namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public interface IConfigWindow<out TConfiguration> : IWindow
        where TConfiguration : class, IPluginConfiguration, new()
    {
        public TConfiguration Config { get; }
    }
}
