using System;
using Serilog.Core;

namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * プラグインのロガーに関する実装を行います。
     */
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        private Logger? logger;

        public Logger Logger => logger ?? throw new InvalidOperationException("Logger はまだ初期化されていません。");
    }
}
