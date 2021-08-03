using System;
using System.IO;
using System.Reflection;

namespace Divination.Common
{
    public static class DivinationEnvironment
    {
        public static readonly string DivinationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Horoscope", "Divination");

        public static readonly string ConfigDirectory = Path.Combine(DivinationDirectory, "Config");
        public static readonly string ConfigBackupDirectory = Path.Combine(ConfigDirectory, "Backup");
        public static readonly string LogDirectory = Path.Combine(DivinationDirectory, "Logs");
        public static readonly string CacheDirectory = Path.Combine(DivinationDirectory, "Cache");

        public static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly GitVersion Version = new GitVersion();
    }
}
