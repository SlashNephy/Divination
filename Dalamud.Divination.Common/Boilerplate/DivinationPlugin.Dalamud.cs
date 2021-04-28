using System;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        private DalamudPluginInterface? @interface;

        public DalamudPluginInterface Interface => @interface ?? throw new InvalidOperationException("Interface はまだ初期化されていません。");
    }
}
