using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Divination.ChatFilter;

public sealed class ChatFilterPlugin : DivinationPlugin<ChatFilterPlugin, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private readonly ChatFilterManager manager = new();

    public ChatFilterPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        PluginLog.Information("Plugin loaded!");
    }

    protected override void ReleaseManaged()
    {
        manager.Dispose();
    }

    string ICommandSupport.MainCommandPrefix => "/filters";
    ConfigWindow<PluginConfig> IConfigWindowSupport<PluginConfig>.CreateConfigWindow() => new PluginConfigWindow();
}