using System;

namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * プラグインの設定に関する実装を行います。
     */
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        private TConfiguration? config;

        public TConfiguration Config => config ?? throw new InvalidOperationException("Config はまだ初期化されていません。");

        /// <summary>
        /// ファイルからプラグインの設定を読み込みます。ファイルが存在しなかった場合、新たなインスタンスを作成します。
        /// </summary>
        /// <returns>Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラスのインスタンス。</returns>
        public TConfiguration LoadConfig()
        {
            return Interface.GetPluginConfig() as TConfiguration ?? new TConfiguration();
        }
    }
}
