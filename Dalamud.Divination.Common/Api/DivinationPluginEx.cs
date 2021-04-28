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
        /// <typeparam name="TPlugin">プラグインのクラス。</typeparam>
        /// <typeparam name="TConfiguration">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
        public static void SaveConfig<TPlugin, TConfiguration>(this DivinationPlugin<TPlugin, TConfiguration> plugin)
            where TPlugin : DivinationPlugin<TPlugin, TConfiguration>
            where TConfiguration : class, IPluginConfiguration, new()
        {
            plugin.Interface.SavePluginConfig(plugin.Config);
        }
    }
}
