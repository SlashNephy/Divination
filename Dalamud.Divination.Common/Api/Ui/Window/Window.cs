namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public abstract class Window : IWindow
    {
        protected bool IsOpen;

        public bool IsDrawing
        {
            get => IsOpen;
            set => IsOpen = value;
        }

        public abstract void Draw();
    }
}
