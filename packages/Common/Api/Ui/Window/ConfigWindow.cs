using System;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Interface;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Ui.Window;

public abstract class ConfigWindow<TConfiguration> : Window, IConfigWindow<TConfiguration>, IDisposable,
    ICommandProvider where TConfiguration : class, IPluginConfiguration, new()
{
    public TConfiguration Config => ConfigManager.Config;
    public DalamudPluginInterface Interface => ConfigManager.Interface;

    public void Dispose()
    {
        UiBuilder.OpenConfigUi -= OnMainCommand;
        UiBuilder.Draw -= OnDraw;
    }

    [Command("")]
    [HiddenCommand(HideInHelp = false)]
    [CommandHelp("{Name} の設定ウィンドウを開きます。")]
    private void OnMainCommand()
    {
        this.Toggle();
    }

    private void OnDraw()
    {
        if (!IsDrawing)
        {
            return;
        }

        Draw();
    }

    internal void EnableHook()
    {
        UiBuilder.OpenConfigUi += OnMainCommand;
        UiBuilder.Draw += OnDraw;
    }

#pragma warning disable 8618
    internal IConfigManager<TConfiguration> ConfigManager { get; set; }
    internal UiBuilder UiBuilder { get; set; }
#pragma warning restore 8618
}
