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
using Dalamud.Game.Gui;
using Dalamud.IoC;
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
        private static TPlugin? _instance;

        /// <summary>
        /// プラグインのインスタンスの静的プロパティ。
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
        public static TPlugin Instance => _instance ?? throw new InvalidOperationException("Instance はまだ初期化されていません。");

        [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public DivinationPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ChatGui chatGui)
        {
            _instance = this as TPlugin ?? throw new TypeAccessException("クラス インスタンスが型パラメータ: TPlugin と一致しません。");

            PreInitialize(pluginInterface, chatGui);

            IsDisposed = false;

            chatClient?.Print("プラグインを読み込みました！");
        }

#if DEBUG
        private DalamudLogger? dalamudLogger;
#endif

        private void PreInitialize(DalamudPluginInterface pluginInterface, ChatGui chatGui)
        {
            logger = DivinationLogger.File(Name);
            @interface = pluginInterface;
            chatClient = new ChatClient(Name, chatGui);

            if (this is ICommandSupport support)
            {
                commandProcessor = new CommandProcessor(Name, support.CommandPrefix, chatGui, chatClient);
                commandProcessor.RegisterCommandsByAttribute(new DirectoryCommands());
            }

            configManager = new ConfigManager<TConfiguration>(pluginInterface, chatClient);
            commandProcessor?.RegisterCommandsByAttribute(new ConfigManager<TConfiguration>.Commands(configManager, CommandProcessor, chatClient));

            versionManager = new VersionManager(
                new GitVersion(Assembly),
                new GitVersion(System.Reflection.Assembly.GetExecutingAssembly()));
            commandProcessor?.RegisterCommandsByAttribute(new VersionManager.Commands(versionManager, chatClient));

            bugReporter = new BugReporter(Name, versionManager, chatClient);
            commandProcessor?.RegisterCommandsByAttribute(new BugReporter.Commands(bugReporter));

#if DEBUG
            dalamudLogger = new DalamudLogger(@interface.GetDalamud());
            dalamudLogger.Subscribe();
#endif

            if (this is ICommandProvider provider)
            {
                commandProcessor?.RegisterCommandsByAttribute(provider);
            }

            logger.Information("プラグイン: {Name} の初期化に成功しました。バージョン = {Version}", Name, versionManager.PluginVersion.InformationalVersion);
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
                DisposeManaged();
            }

            DisposeUnmanaged();

            PostDispose(disposing);
        }

        private void PostDispose(bool disposing)
        {
            if (disposing)
            {
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
