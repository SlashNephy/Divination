namespace Dalamud.Divination.Common.Api.Ui.Window
{
    public static class WindowEx
    {
        public static void Toggle(this IWindow window)
        {
            window.IsDrawing = !window.IsDrawing;
        }
    }
}
