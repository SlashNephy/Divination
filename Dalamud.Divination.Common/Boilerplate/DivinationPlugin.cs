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
    /// <typeparam name="TPlugin">プラグインのクラス。</typeparam>
    /// <typeparam name="TConfiguration">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration> : IDivinationPlugin<TConfiguration>, IDivinationPluginApi<TConfiguration>, IDisposable
        where TPlugin : DivinationPlugin<TPlugin, TConfiguration>
        where TConfiguration : class, IPluginConfiguration
    {
        private static TPlugin? _instance;

        /// <summary>
        /// プラグインのインスタンスの静的プロパティ。
        /// </summary>
        protected static TPlugin Instance => _instance ?? throw new InvalidOperationException("Instance はまだ初期化されていません。");

        [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        protected DivinationPlugin()
        {
            _instance = this as TPlugin ?? throw new TypeAccessException("クラス インスタンスが型パラメータ: TPlugin と一致しません。");
        }

        /// <summary>
        /// Dalamud プラグインを初期化します。
        /// Divination プラグインから呼び出されることは想定されていません。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        /// <param name="pluginInterface">Dalamud.Plugin.DalamudPluginInterface</param>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            logger = DivinationLogger.Of(Name);

            @interface = pluginInterface;
            config = LoadConfig();

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
            this.SaveConfig();
        }
    }
}
