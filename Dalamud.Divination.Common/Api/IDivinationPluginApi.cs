using Dalamud.Configuration;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api
{
    /// <summary>
    /// 各 Divination プラグインが実装している各種 API のインターフェイスです。
    /// </summary>
    public interface IDivinationPluginApi<out TConfiguration> where TConfiguration : class, IPluginConfiguration
    {
        /// <summary>
        /// プラグインのロガー。
        /// </summary>
        public Serilog.Core.Logger Logger { get; }

        /// <summary>
        /// DalamudPluginInterface
        /// </summary>
        public DalamudPluginInterface Interface { get; }

        /// <summary>
        /// Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラスのインスタンス。
        /// </summary>
        public TConfiguration Config { get; }

        /// <summary>
        /// プラグインのバージョン情報。
        /// </summary>
        public GitVersion Version { get; }

        /// <summary>
        /// Dalamud.Divination.Common のバージョン情報。
        /// </summary>
        public GitVersion CommonVersion { get; }
    }
}
