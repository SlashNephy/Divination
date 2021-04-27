using System.Reflection;
using Dalamud.Configuration;

namespace Dalamud.Divination.Common.Boilerplate
{
    /// <summary>
    /// Divination プラグインの基本インターフェイスです。
    /// 各プラグインが実装を行う必要があります。
    /// Dalamud.Plugin.IDalamudPlugin のインターフェイスに対応します。
    /// </summary>
    /// <typeparam name="TC">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
    public interface IDivinationPlugin<out TC> where TC : class, IPluginConfiguration
    {
        /// <summary>
        /// プラグインの名前を設定します。この名前は Dalamud に通知されます。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// プラグインのコードが格納されているアセンブリを設定します。
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// プラグインロード時の処理を記述します。
        /// </summary>
        public void Load();

        /// <summary>
        /// プラグインアンロード時の処理を記述します。
        /// </summary>
        public void Unload();

        /// <summary>
        /// プラグインの設定を読み込みます。
        /// </summary>
        /// <returns>Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラスのインスタンス。</returns>
        public TC LoadConfig();
    }
}
