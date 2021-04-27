using System;
using System.IO;

namespace Dalamud.Divination.Common
{
    public static class DivinationEnvironment
    {
        public static string DivinationDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Horoscope", "Divination");
        public static string ConfigDirectory => Path.Combine(DivinationDirectory, "Config");
        public static string ConfigBackupDirectory => Path.Combine(ConfigDirectory, "Backup");
        public static string LogDirectory => Path.Combine(DivinationDirectory, "Logs");
        public static string CacheDirectory => Path.Combine(DivinationDirectory, "Cache");

        public static readonly GitVersion Version = new();
    }
}
