using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using Divination.SseClient.Handlers;

namespace Divination.SseClient;

public sealed class SseClient : DivinationPlugin<SseClient, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    public readonly SseConnectionManager Connection = new();

    public SseClient(DalamudPluginInterface pluginInterface) : base(pluginInterface)
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
