using System;
using System.IO;

namespace Dalamud.Divination.Common.Api
{
    /// <summary>
    ///     Divination プラグインで使用される各種変数を提供します。
    /// </summary>
    public static class DivinationEnvironment
    {
        /// <summary>
        ///     Divination のホームディレクトリへのパス。
        /// </summary>
        public static string DivinationDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Horoscope",
                "Divination");

        /// <summary>
        ///     Divination のキャッシュディレクトリへのパス。
        /// </summary>
        public static string CacheDirectory => Path.Combine(DivinationDirectory, "Cache");

        /// <summary>
        ///     FFXIV ゲームディレクトリへのパス。
        ///     通常、"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\game" になります。
        ///     このプロパティは Dalamud 上でのみ機能します。
        /// </summary>
        public static string GameDirectory => Path.GetDirectoryName(Environment.ProcessPath)!;

        /// <summary>
        ///     XIVLauncher のホームディレクトリへのパス。
        /// </summary>
        public static string XivLauncherDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher");
    }
}
