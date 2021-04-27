namespace Dalamud.Divination.Common.Boilerplate
{
    /*
     * プラグインの設定に関する実装を行います。
     */
    public abstract partial class DivinationPlugin<TC>
    {
#pragma warning disable 8618
        public TC Config { get; private set; }
#pragma warning restore 8618
    }
}
