using System.Reflection;

namespace Dalamud.Divination.Common.Boilerplate
{
    /// <summary>
    /// Divination プラグインの基本インターフェイスです。
    /// Dalamud.Plugin.IDalamudPlugin のインターフェイスに対応します。
    /// </summary>
    public interface IDivinationPlugin
    {
        /// <summary>
        /// プラグインの名前を設定します。この名前は Dalamud に通知されます。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// プラグインのコードが格納されているアセンブリを設定します。
        /// </summary>
        public Assembly Assembly { get; }

        public void DisposeManaged();
        public void DisposeUnmanaged();
    }
}
