namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * プラグインの設定に関する実装を行います。
     */
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
#pragma warning disable 8618
        public TConfiguration Config { get; private set; }
#pragma warning restore 8618
    }
}
