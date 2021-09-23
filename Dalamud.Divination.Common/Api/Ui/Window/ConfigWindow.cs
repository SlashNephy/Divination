using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Command.Attributes;
using Dalamud.Divination.Common.Api.Config;

namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public abstract class ConfigWindow<TConfiguration> : Window, ICommandProvider where TConfiguration : class, IPluginConfiguration, new()
    {
#pragma warning disable 8618
        internal IConfigManager<TConfiguration> ConfigManager { get; set; }
#pragma warning restore 8618

        public TConfiguration Config => ConfigManager.Config;

        [Command("")]
        [CommandHelp("プラグインの設定ウィンドウを開きます。")]
        private void OnMainCommand()
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
    }
}
