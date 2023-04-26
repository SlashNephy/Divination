using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Divination.Common
{
    public static class DivinationLoggerFactory
    {
        public static IDivinationLogger Create()
        {
            return CreateByName(DivinationEnvironment.AssemblyName);
        }

        public static IDivinationLogger Create(string name)
        {
            return CreateByName($"{DivinationEnvironment.AssemblyName}#{name}");
        }

        public static IDivinationLogger Create(object instance)
        {
            return CreateByName(instance.GetType().Namespace!);
        }

        public static IDivinationLogger CreateByName(string name)
        {
#if DEBUG
            return new ConsoleLogger(name);
#else
            return new FileLogger(name);
#endif
        }
    }

    public interface IDivinationLogger : IDisposable
    {
        public string Name { get; }

        public void Append(LogLevel level, params object?[] messages);
    }

    internal class FileLogger : IDivinationLogger
    {
        public string Name { get; }

        public FileLogger(string name)
        {
            Name = name;
        }

        private StreamWriter? writer;
        private readonly object writerLock = new object();

        private StreamWriter CreateWriter()
        {
            if (!Directory.Exists(DivinationEnvironment.LogDirectory))
            {
                Directory.CreateDirectory(DivinationEnvironment.LogDirectory);
            }

            var path = Path.Combine(DivinationEnvironment.LogDirectory, $"{Name}.log");
            var file = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            return new StreamWriter(file)
            {
                NewLine = "\n",
                AutoFlush = true
            };
        }

        public void Append(LogLevel level, params object?[] messages)
        {
            if (LogLevel.Info < level)
            {
                return;
            }

            lock (writerLock)
            {
                writer ??= CreateWriter();

                var line = this.CreateLogLine(level, messages);
                writer.WriteLine(line);
            }
        }

        public void Dispose()
        {
            writer?.Dispose();
        }
    }

    public class ConsoleLogger : IDivinationLogger
    {
        public string Name { get; }
        private readonly FileLogger fileLogger;
        public readonly bool HasConsole;
        public bool OutputToFile = true;

        public ConsoleLogger(string name)
        {
            Name = name;
            fileLogger = new FileLogger(name);
            HasConsole = CreateConsole();
        }

        public void Append(LogLevel level, params object?[] messages)
        {
            var line = this.CreateLogLine(level, messages);

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

            Console.WriteLine(line);

            if (colorChanged)
            {
                Console.ResetColor();
            }

            if (OutputToFile && level < LogLevel.Info)
            {
                fileLogger.Append(level, messages);
            }
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

    public enum LogLevel
    {
        Error, Warn, Info, Debug, Trace
    }

    public static class DivinationLoggerEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string CreateLogLine(this IDivinationLogger logger, LogLevel level, params object?[] messages)
        {
            return $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}] [{Enum.GetName(typeof(LogLevel), level),-5}] [{logger.Name}] {string.Join("\n", ToStringFuzzy(messages))}";
        }

        private static IEnumerable<string> ToStringFuzzy(IEnumerable<object?> objects)
        {
            return objects.Select(o =>
            {
                return o switch
                {
                    string text => text,
                    IEnumerable enumerable => string.Join(", ", ToStringFuzzy(enumerable.Cast<object?>())),
                    _ => o?.ToString() ?? "null"
                };
            });
        }

        public static void Error(this IDivinationLogger logger, params object?[] messages)
        {
            logger.Append(LogLevel.Error, messages);
        }

        public static void Warn(this IDivinationLogger logger, params object?[] messages)
        {
            logger.Append(LogLevel.Warn, messages);
        }

        public static void Info(this IDivinationLogger logger, params object?[] messages)
        {
            logger.Append(LogLevel.Info, messages);
        }

        public static void Debug(this IDivinationLogger logger, params object?[] messages)
        {
            logger.Append(LogLevel.Debug, messages);
        }

        public static void Trace(this IDivinationLogger logger, params object?[] messages)
        {
            logger.Append(LogLevel.Trace, messages);
        }

        private static void AppendException(this IDivinationLogger logger, LogLevel level, Exception e, params object?[] messages)
        {
            var parameters = messages.Append(e.ToString());
            logger.Append(level, parameters.ToArray());
        }

        public static void Error(this IDivinationLogger logger, Exception e, params object?[] messages)
        {
            logger.AppendException(LogLevel.Error, e, messages);
        }

        public static void Warn(this IDivinationLogger logger, Exception e, params object?[] messages)
        {
            logger.AppendException(LogLevel.Warn, e, messages);
        }
    }
}
