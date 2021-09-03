using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Divination.Template
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class Win32Api
    {
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MONITORPOWER = 0xF170;
        public const int MONITOR_OFF = 2;

        [DllImport("user32.dll")]
        public static extern bool SendMessage(int hWnd, int msg, int wParam, int lParam);

        [DllImport("PowrProf.dll", CharSet = CharSet.Unicode)]
        public static extern uint PowerSetActiveScheme(IntPtr rootPowerKey, [MarshalAs(UnmanagedType.LPStruct)] Guid schemeGuid);
    }
}
