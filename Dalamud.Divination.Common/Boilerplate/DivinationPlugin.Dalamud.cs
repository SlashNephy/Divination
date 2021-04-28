using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
#pragma warning disable 8618
        public DalamudPluginInterface Interface { get; private set; }
#pragma warning restore 8618
    }
}
