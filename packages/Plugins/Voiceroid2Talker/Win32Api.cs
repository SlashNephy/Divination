using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Divination.Voiceroid2Talker;

internal static class Win32Api
{
    public static bool IsGameClientActive()
    {
        return GetForegroundProcess()?.ProcessName == "ffxiv_dx11";
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    private static Process? GetForegroundProcess()
    {
        var window = GetForegroundWindow();
        if (window == IntPtr.Zero)
        {
            return null;
        }

        GetWindowThreadProcessId(window, out var pid);

        return Process.GetProcessById((int)pid);
    }
}