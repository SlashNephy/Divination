using Dalamud.Divination.Common.Api.Command;

namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public abstract class ConfigWindow : IWindow
    {
        public bool IsDrawing { get; set; }

        public abstract void Draw();

        [Command("", Help = "プラグインの設定ウィンドウを開きます。")]
        private void OnMainCommand()
        {
            this.Toggle();
        }
    }
}
