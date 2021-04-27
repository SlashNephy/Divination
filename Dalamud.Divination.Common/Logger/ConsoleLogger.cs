using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Dalamud.Divination.Common.Logger
{
    internal class ConsoleLogger : IDivinationLogger
    {
        public string Name { get; }
        private readonly FileLogger fileLogger;

        public ConsoleLogger(string name)
        {
            Name = name;
            fileLogger = new FileLogger(name);
            HasConsole = CreateConsole();
        }

        public void Append(LogLevel level, string message)
        {
            var colorChanged = false;
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    colorChanged = true;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    colorChanged = true;
                    break;
            }

            Console.WriteLine(message);

            if (colorChanged)
            {
                Console.ResetColor();
            }

            if (fileLogger.IsEnabledFor(level))
            {
                fileLogger.Append(level, message);
            }
        }

        public bool IsEnabledFor(LogLevel level)
        {
            return true;
        }

        private static bool CreateConsole()
        {
            if (!WinApi.AttachConsole(WinApi.AttachParentProcess) && WinApi.AllocConsole())
            {
                var stdHandle = WinApi.GetStdHandle(WinApi.StdOutputHandle);
                var safeStdHandle = new SafeFileHandle(stdHandle, true);
                var stdStream = new FileStream(safeStdHandle, FileAccess.Write);
                var stdWriter = new StreamWriter(stdStream, Encoding.Default)
                {
                    AutoFlush = true
                };

                Console.SetOut(stdWriter);
                Console.Title = "Debug Console";
                Console.SetWindowSize(164, 42);

                var handle = WinApi.GetConsoleWindow();
                WinApi.SetWindowPos(handle, new IntPtr(WinApi.HwndTopmost), 0, 0, 0, 0, WinApi.SwpNomove | WinApi.SwpNosize);

                Console.WriteLine("Debug Console initialized!");
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            fileLogger.Dispose();
        }

        private static class WinApi
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
