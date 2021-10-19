using System;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Interface;

namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public abstract class ConfigWindow<TConfiguration> : Window, IDisposable, ICommandProvider where TConfiguration : class, IPluginConfiguration, new()
    {
#pragma warning disable 8618
        internal IConfigManager<TConfiguration> ConfigManager { get; set; }
        internal UiBuilder UiBuilder { get; set; }
#pragma warning restore 8618

        public TConfiguration Config => ConfigManager.Config;

        [Command("")]
        [CommandHelp("{Name} の設定ウィンドウを開きます。")]
        internal void OnMainCommand()
        {
            this.Toggle();
        }

        internal void OnDraw()
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

        public void Dispose()
        {
            UiBuilder.OpenConfigUi -= OnMainCommand;
            UiBuilder.Draw -= OnDraw;
        }
    }
}
