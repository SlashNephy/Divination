using System.IO;

namespace Dalamud.Divination.Common.Logger
{
    internal class FileLogger : IDivinationLogger
    {
        private const LogLevel MinimalLevel  = LogLevel.Info;

        public string Name { get; }

        public FileLogger(string name)
        {
            Name = name;
        }

        private StreamWriter? writer;
        private readonly object writerLock = new();

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

        public void Append(LogLevel level, string message)
        {
            lock (writerLock)
            {
                writer ??= CreateWriter();

                writer.WriteLine(message);
            }
        }

        public bool IsEnabledFor(LogLevel level)
        {
            return level <= MinimalLevel;
        }

        public void Dispose()
        {
            writer?.Dispose();
        }
    }
}
