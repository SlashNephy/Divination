using System;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api.Dalamud;

namespace Dalamud.Divination.Common.Api
{
    /// <summary>
    /// 各 Divination プラグインが実装している各種 API のインターフェイスです。
    /// </summary>
    public interface IDivinationPluginApi<TConfiguration> : IDisposable where TConfiguration : class, IPluginConfiguration, new()
    {
        /// <summary>
        /// プラグインの名前を取得します。この名前は Dalamud に通知されます。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        public string Name { get; }

        public bool IsDisposed { get; }

        /// <summary>
        /// プラグインのロガー。
        /// </summary>
        public Serilog.Core.Logger Logger { get; }

        /// <summary>
        /// Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラスのインスタンス。
        /// </summary>
        public TConfiguration Config { get; }

        public DalamudApi Dalamud { get; }

        public DivinationApi<TConfiguration> Divination { get; }

        /// <summary>
        /// プラグインのコードが格納されているアセンブリを取得します。
        /// </summary>
        public Assembly Assembly { get; }
    }
}
