using System;
using System.Runtime.InteropServices;

namespace Divination.ACT.MobKillsCounter.Util
{
    internal static class Win32ApiUtils
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        private const int WsExTransparent = 0x00000020;
        private const int GwlExstyle = -20;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);


        public static void DragMove(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, WmNclbuttondown, HtCaption, 0);
        }

        public static void SetWS_EX_TRANSPARENT(IntPtr handle, bool value)
        {
            var origStyle = GetWindowLong(handle, GwlExstyle);

            int style;
            if (value)
            {
                style = origStyle | WsExTransparent;
            }
            else
            {
                style = origStyle & ~WsExTransparent;
            }

            SetWindowLong(handle, GwlExstyle, style);
        }
    }
}
