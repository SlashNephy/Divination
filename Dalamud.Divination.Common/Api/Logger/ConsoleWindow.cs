using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Dalamud.Divination.Common.Api.Logger
{
    /// <summary>
    /// コンソールウィンドウに関するメソッドを実装した静的クラスです。
    /// </summary>
    public static class ConsoleWindow
    {
        private static DalamudLogger? DalamudLogger { get; set; }

        /// <summary>
        /// コンソールウィンドウを表示する。
        /// </summary>
        /// <returns>コンソールウィンドウが新たに作成されたかどうか。</returns>
        public static bool Show()
        {
            if (!Win32Api.AttachConsole(Win32Api.AttachParentProcess) && Win32Api.AllocConsole())
            {
                var stdHandle = Win32Api.GetStdHandle(Win32Api.StdOutputHandle);
                var safeStdHandle = new SafeFileHandle(stdHandle, true);
                var stdStream = new FileStream(safeStdHandle, FileAccess.Write);
                var stdWriter = new StreamWriter(stdStream, Encoding.Default)
                {
                    AutoFlush = true
                };

                Console.SetOut(stdWriter);
                Console.Title = "Divination Debug Console";
                Console.SetWindowSize(164, 42);

                var handle = Win32Api.GetConsoleWindow();
                Win32Api.SetWindowPos(handle, new IntPtr(Win32Api.HwndTopmost), 0, 0, 0, 0, Win32Api.SwpNomove | Win32Api.SwpNosize);

#if DEBUG
                DalamudLogger = new DalamudLogger();
                DalamudLogger.Subscribe();
#endif

                Console.WriteLine("Divination Debug Console initialized!");
                return true;
            }

            return false;
        }

        private static class Win32Api
        {
            public const int AttachParentProcess = -1;

            public const int StdOutputHandle = -11;

            public const int HwndTopmost = -1;
            public const int SwpNosize = 0x0001;
            public const int SwpNomove = 0x0002;

            [DllImport("Kernel32.dll")]
            public static extern bool AttachConsole(int processId);

            [DllImport("kernel32.dll")]
            public static extern bool AllocConsole();


            [DllImport("kernel32.dll")]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);
        }
    }
}
