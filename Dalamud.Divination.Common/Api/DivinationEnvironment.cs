using System;
using System.IO;

namespace Dalamud.Divination.Common.Api
{
    /// <summary>
    /// Divination プラグインで使用される各種変数を提供します。
    /// </summary>
    public static class DivinationEnvironment
    {
        /// <summary>
        /// Divination のホームディレクトリへのパス。
        /// </summary>
        public static string DivinationDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Horoscope", "Divination");

        /// <summary>
        /// Divination の設定ディレクトリへのパス。
        /// </summary>
        public static string ConfigDirectory => Path.Combine(DivinationDirectory, "Config");

        /// <summary>
        /// Divination の設定のバックアップディレクトリへのパス。
        /// </summary>
        public static string ConfigBackupDirectory => Path.Combine(ConfigDirectory, "Backup");

        /// <summary>
        /// Divination のログディレクトリへのパス。
        /// </summary>
        public static string LogDirectory => Path.Combine(DivinationDirectory, "Logs");

        /// <summary>
        /// Divination のキャッシュディレクトリへのパス。
        /// </summary>
        public static string CacheDirectory => Path.Combine(DivinationDirectory, "Cache");
    }
}
