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
    }
}
