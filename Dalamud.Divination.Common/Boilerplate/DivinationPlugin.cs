using System;
using System.Diagnostics.CodeAnalysis;
using Dalamud.Configuration;
using Dalamud.Divination.Common.Api;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Logger;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    /// <summary>
    /// Divination プラグインのボイラープレートを提供します。
    /// このクラスを継承することで Dalamud 互換のプラグインを作成できます。
    /// </summary>
    /// <typeparam name="TPlugin">プラグインのクラス。</typeparam>
    /// <typeparam name="TConfiguration">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration> : IDivinationPlugin, IDivinationPluginApi<TConfiguration>, IDisposable
        where TPlugin : DivinationPlugin<TPlugin, TConfiguration>
        where TConfiguration : class, IPluginConfiguration, new()
    {
        #region Static Property

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

        #endregion

#if DEBUG
        private DalamudLogger? dalamudLogger;
#endif

        /// <summary>
        /// Dalamud プラグインを初期化します。
        /// Divination プラグインから呼び出されることは想定されていません。
        /// Dalamud.Plugin.IDalamudPlugin のために実装されています。
        /// </summary>
        /// <param name="pluginInterface">Dalamud.Plugin.DalamudPluginInterface</param>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            logger = DivinationLogger.File(Name);
            @interface = pluginInterface;
            chatClient = new ChatClient(Name, pluginInterface.Framework.Gui.Chat);

            commandProcessor = new CommandProcessor(Name, CommandPrefix ?? Name, pluginInterface.Framework.Gui.Chat, chatClient);

            configManager = new ConfigManager<TConfiguration>(pluginInterface, chatClient, commandProcessor);
            commandProcessor.RegisterCommandsByAttribute(configManager);

            versionManager = new VersionManager(
                new GitVersion(Assembly),
                new GitVersion(System.Reflection.Assembly.GetExecutingAssembly()),
                chatClient);
            commandProcessor.RegisterCommandsByAttribute(versionManager);

            bugReporter = new BugReporter(Name, versionManager, chatClient);
            commandProcessor.RegisterCommandsByAttribute(bugReporter);

#if DEBUG
            dalamudLogger = new DalamudLogger(@interface.GetDalamud());
            dalamudLogger.Subscribe();
#endif

            IsDisposed = false;

            commandProcessor.RegisterCommandsByAttribute(this);
            Load();

            logger?.Information("プラグイン: {Name} の初期化に成功しました。バージョン = {Version}", Name, versionManager.PluginVersion.InformationalVersion);
            chatClient?.Print("プラグインを読み込みました！");
        }

        #region Dispose Pattern

        /// <summary>
        /// Divination プラグイン内で確保されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IsDisposed = true;

                Unload();

                configManager?.Dispose();
                versionManager?.Dispose();
                commandProcessor?.Dispose();
                bugReporter?.Dispose();

                chatClient?.Print("プラグインを停止しました。");
                chatClient?.Dispose();

#if DEBUG
                dalamudLogger?.Dispose();
#endif

                @interface?.Dispose();

                logger?.Information("Plugin was disposed. Good-bye!");
                logger?.Dispose();
            }
        }

        ~DivinationPlugin()
        {
            Dispose(false);
        }

        #endregion
    }
}
