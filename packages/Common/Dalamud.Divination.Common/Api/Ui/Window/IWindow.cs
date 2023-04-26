namespace Dalamud.Divination.Common.Api.Ui.Window;

public interface IWindow
{
    public bool IsDrawing { get; set; }

    public void Draw();
}
