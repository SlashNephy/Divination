using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api
{
    /// <summary>
    /// 各 Divination プラグインが実装している各種 API のインターフェイスです。
    /// </summary>
    public interface IDivinationPluginApi<out TConfiguration> where TConfiguration : class, IPluginConfiguration
    {
        public bool IsDisposed { get; }

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

        public IConfigManager<TConfiguration> ConfigManager { get; }

        /// <summary>
        /// プラグインのバージョン情報。
        /// </summary>
        public IGitVersion Version { get; }

        /// <summary>
        /// Dalamud.Divination.Common のバージョン情報。
        /// </summary>
        public IGitVersion LibraryVersion { get; }

        public IVersionManager VersionManager { get; }

        public IChatClient ChatClient { get; }

        public ICommandProcessor CommandProcessor { get; }

        public IBugReporter BugReporter { get; }
    }
}
