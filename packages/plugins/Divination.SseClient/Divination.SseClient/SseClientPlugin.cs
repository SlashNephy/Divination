using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using Divination.SseClient.Handlers;

namespace Divination.SseClient
{
    public sealed class SseClientPlugin : DivinationPlugin<SseClientPlugin, PluginConfig>,
        IDalamudPlugin,
        ICommandSupport,
        IConfigWindowSupport<PluginConfig>
    {
        public readonly SseConnectionManager Connection = new();

        public SseClientPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            Connection.Connect();
        }

        protected override void ReleaseManaged()
        {
            Connection.Dispose();
        }

        string ICommandSupport.MainCommandPrefix => "/sse";
        ConfigWindow<PluginConfig> IConfigWindowSupport<PluginConfig>.CreateConfigWindow() => new PluginConfigWindow();
    }
}
