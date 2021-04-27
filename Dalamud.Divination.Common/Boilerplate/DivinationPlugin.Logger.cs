namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * プラグインのロガーに関する実装を行います。
     */
    public abstract partial class DivinationPlugin<TC>
    {
#pragma warning disable 8618
        public Serilog.Core.Logger Logger { get; private set; }
#pragma warning restore 8618
    }
}
