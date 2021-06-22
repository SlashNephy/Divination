using System;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Command;
using Dalamud.Divination.Common.Api.Config;
using Dalamud.Divination.Common.Api.Reporter;
using Dalamud.Divination.Common.Api.Version;
using Dalamud.Plugin;
using Serilog.Core;

namespace Dalamud.Divination.Common.Boilerplate
{
    public abstract partial class DivinationPlugin<TPlugin, TConfiguration>
    {
        public bool IsDisposed { get; private set; }

        #region Logger

        private Logger? logger;
        public Logger Logger => logger ?? throw new InvalidOperationException("Logger はまだ初期化されていません。");

        #endregion

        #region Interface

        private DalamudPluginInterface? @interface;
        public DalamudPluginInterface Interface => @interface ?? throw new InvalidOperationException("Interface はまだ初期化されていません。");

        #endregion

        #region Config

        public TConfiguration Config => ConfigManager.Config;

        #endregion

        #region ConfigManager

        private IConfigManager<TConfiguration>? configManager;
        public IConfigManager<TConfiguration> ConfigManager => configManager ?? throw new InvalidOperationException("ConfigManager はまだ初期化されていません。");

        #endregion

        #region Version

        public IGitVersion Version => VersionManager.PluginVersion;

        #endregion

        #region LibraryVersion

        public IGitVersion LibraryVersion => VersionManager.LibraryVersion;

        #endregion

        #region VersionManager

        private IVersionManager? versionManager;
        public IVersionManager VersionManager => versionManager ?? throw new InvalidOperationException("VersionManager はまだ初期化されていません。");

        #endregion

        #region ChatClient

        private IChatClient? chatClient;
        public IChatClient ChatClient => chatClient ?? throw new InvalidOperationException("ChatClient はまだ初期化されていません。");

        #endregion

        #region CommandProcessor

        private ICommandProcessor? commandProcessor;
        public ICommandProcessor CommandProcessor => commandProcessor ?? throw new InvalidOperationException("CommandProcessor はまだ初期化されていません。");

        #endregion

        #region BugReporter

        private IBugReporter? bugReporter;
        public IBugReporter BugReporter => bugReporter ?? throw new InvalidOperationException("BugReporter はまだ初期化されていません。");

        #endregion
    }
}
