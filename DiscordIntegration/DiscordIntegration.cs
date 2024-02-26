using System.Timers;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using Divination.DiscordIntegration.Discord;
using Divination.DiscordIntegration.Ipc;

namespace Divination.DiscordIntegration;

public partial class DiscordIntegration : DivinationPlugin<DiscordIntegration, PluginConfig, PluginDefinition>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>,
    IDefinitionSupport
{
    private readonly DiscordApi discord = new();
    private readonly Timer timer = new(3000);

    public DiscordIntegration(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        SetDefaultPresence();

        timer.Elapsed += OnElapsed;
        timer.Start();

        IpcManager.Register();
    }

    public string MainCommandPrefix => "/discord";
    public string DefinitionUrl => "https://github.com/horoscope-dev/Divination.Definitions/raw/master/dist/DiscordIntegration.json";

    public ConfigWindow<PluginConfig> CreateConfigWindow()
    {
        return new PluginConfigWindow();
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        IpcManager.Unregister();
        timer.Dispose();
        discord.Dispose();
    }
}
