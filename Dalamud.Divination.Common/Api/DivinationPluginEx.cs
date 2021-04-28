using Dalamud.Configuration;
using Dalamud.Divination.Common.Boilerplate;

namespace Dalamud.Divination.Common.Api
{
    /// <summary>
    /// DivinationPlugin に関する拡張メソッドが定義されている静的クラスです。
    /// </summary>
    public static class DivinationPluginEx
    {
        /// <summary>
        /// 現在のプラグインの設定インスタンスをファイルに保存します。
        /// プラグインの終了時に自動的に呼ばれます。
        /// </summary>
        /// <typeparam name="TC">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
        public static void SaveConfig<TC>(this DivinationPlugin<TC> plugin) where TC : class, IPluginConfiguration
        {
            plugin.Interface.SavePluginConfig(plugin.Config);
        }
    }
}
