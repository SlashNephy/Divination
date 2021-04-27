using System;
using System.Diagnostics.CodeAnalysis;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    /// <summary>
    /// Divination プラグインのボイラープレートを提供します。
    /// このクラスを継承することで Dalamud 互換のプラグインを作成できます。
    /// </summary>
    /// <typeparam name="TC">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
    public abstract partial class DivinationPlugin<TC> : IDivinationPlugin<TC>, IDivinationPluginApi<TC>, IDisposable where TC : class, IPluginConfiguration
    {
        /// <summary>
        /// Dalamud プラグインを初期化します。
        /// Divination プラグインから呼び出されることは想定されていません。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        /// <param name="pluginInterface">Dalamud.Plugin.DalamudPluginInterface</param>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            Logger = DivinationLogger.Of(Name);

            Interface = pluginInterface;
            Config = LoadConfig();
            Version = new GitVersion(Assembly);

            Load();

            Logger.Information("プラグイン: {Name} の初期化に成功しました。", Name);
        }

        /// <summary>
        /// Divination プラグイン内で確保されているすべてのリソースを解放します。
        /// Divination プラグインから呼び出されることは想定されていません。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        public void Dispose()
        {
            Unload();
        }
    }
}
