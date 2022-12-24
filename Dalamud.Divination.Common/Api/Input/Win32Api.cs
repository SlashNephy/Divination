using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Dalamud.Divination.Common.Api.Input;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Win32Api
{
    public const ushort WmKeydown = 0x0100;
    public const ushort WmKeyup = 0x0101;

    public const byte VkBack = 0x08;
    public const byte VkTab = 0x09;
    public const byte VkEnter = 0x0D;
    public const byte VkShift = 0x10;
    public const byte VkControl = 0x11;
    public const byte VkAlt = 0x12;
    public const byte VkEscape = 0x1B;
    public const byte VkSpace = 0x20;
    public const byte Vk0 = 0x30;
    public const byte Vk9 = 0x39;
    public const byte VkA = 0x41;
    public const byte VkZ = 0x5A;
    public const byte VkNum0 = 0x60;
    public const byte VkNum9 = 0x69;
    public const byte VkF1 = 0x70;
    public const byte VkF24 = 0x87;

    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindowEx(IntPtr parentWnd,
        IntPtr previousWnd,
        string lpClassName,
        string lpWindowText);

    [DllImport("user32.dll")]
    public static extern int ToUnicode(uint virtualKeyCode,
        uint scanCode,
        byte[] keyboardState,
        [Out] [MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer,
        int bufferSize,
        uint flags);

    public static IntPtr GetGameWindowHandle()
    {
        var process = Process.GetCurrentProcess();
        var hWnd = IntPtr.Zero;

        while ((hWnd = FindWindowEx(IntPtr.Zero, hWnd, "FFXIVGAME", "FINAL FANTASY XIV")) != IntPtr.Zero)
        {
            GetWindowThreadProcessId(hWnd, out var processId);
            if (processId == process.Id)
            {
                return hWnd;
            }
        }

        return IntPtr.Zero;
    }
}
