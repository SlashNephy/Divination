﻿using System.Timers;
using Dalamud.Divination.Common.Api.Ui.Window;
using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Divination.Common.Boilerplate.Features;
using Dalamud.Plugin;
using Divination.DiscordIntegration.Ipc;

namespace Divination.DiscordIntegration;

public partial class DiscordIntegration : DivinationPlugin<DiscordIntegration, PluginConfig>,
    IDalamudPlugin,
    ICommandSupport,
    IConfigWindowSupport<PluginConfig>
{
    private readonly Timer timer = new(3000);

    public DiscordIntegration(IDalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        Config = pluginInterface.GetPluginConfig() as PluginConfig ?? new PluginConfig();
        SetDefaultPresence();

        timer.Elapsed += OnElapsed;
        timer.Start();

        IpcManager.Register();
    }

    public string MainCommandPrefix => "/discord";

    public ConfigWindow<PluginConfig> CreateConfigWindow()
    {
        return new PluginConfigWindow();
    }

    protected override void ReleaseManaged()
    {
        Dalamud.PluginInterface.SavePluginConfig(Config);
        IpcManager.Unregister();
        timer.Dispose();
    }
}
