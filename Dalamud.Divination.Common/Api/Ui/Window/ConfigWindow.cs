using Dalamud.Divination.Common.Api.Command;

namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public class ConfigWindow : IWindow
    {
        public bool IsDrawing { get; set; }

        [Command("", Help = "プラグインの設定ウィンドウを開きます。")]
        private void OnMainCommand()
        {
            this.Toggle();
        }
    }
}
