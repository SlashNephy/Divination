using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Dalamud.Configuration;
using Dalamud.Data;
using Dalamud.Divination.Common.Api;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Divination.Common.Api.Definition;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Aetherytes;
using Dalamud.Game.ClientState.Buddy;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.GamePad;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Boilerplate
{
    /// <summary>
    ///     Divination プラグインのボイラープレートを提供します。
    ///     このクラスを継承することで Dalamud 互換のプラグインを作成できます。
    /// </summary>
    /// <typeparam name="TPlugin">プラグインのクラス。</typeparam>
    /// <typeparam name="TConfiguration">Dalamud.Configuration.IPluginConfiguration を実装したプラグイン設定クラス。</typeparam>
    /// <typeparam name="TDefinition">プラグインの外部定義クラス。</typeparam>
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public abstract class
        DivinationPlugin<TPlugin, TConfiguration, TDefinition> : IDivinationPluginApi<TConfiguration, TDefinition>
        where TPlugin : DivinationPlugin<TPlugin, TConfiguration, TDefinition>
        where TConfiguration : class, IPluginConfiguration, new()
        where TDefinition : DefinitionContainer, new()
    {
#pragma warning disable CS8618
        /// <summary>
        ///     プラグインのインスタンスの静的プロパティ。
        /// </summary>
        public static TPlugin Instance { get; private set; }

        protected DivinationPlugin()
#pragma warning restore CS8618
        {
            Instance = this as TPlugin ?? throw new TypeAccessException("クラス インスタンスが型パラメータ: TPlugin と一致しません。");
            IsDisposed = false;
            Divination = new DivinationApi<TConfiguration, TDefinition>(Assembly, this);

            PluginLog.Information("プラグイン: {Name} の初期化に成功しました。バージョン = {Version}",
                Name,
                Divination.Version.Plugin.InformationalVersion);
        }

        public string Name => $"Divination.{Instance.GetType().Name.Replace("Plugin", string.Empty)}";
        public bool IsDisposed { get; private set; }
        public TConfiguration Config => Divination.Config.Config;
        public TDefinition? Definition => Divination.Definition?.Container;
        [Obsolete("いずれ廃止する、同じプロパティはプラグインクラスに定義されている")]
        public IDalamudApi Dalamud => this;
        public IDivinationApi<TConfiguration, TDefinition> Divination { get; }
        public Assembly Assembly => Instance.GetType().Assembly;

        #region IoC

        [PluginService]
        [RequiredVersion("1.0")]
        public DalamudPluginInterface PluginInterface { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public DataManager DataManager { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ChatHandlers ChatHandlers { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public Framework Framework { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public SigScanner SigScanner { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ClientState ClientState { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public AetheryteList AetheryteList { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public BuddyList BuddyList { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public Condition Condition { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public FateTable FateTable { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public GamepadState GamepadState { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public JobGauges JobGauges { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public KeyState KeyState { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ObjectTable ObjectTable { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public TargetManager TargetManager { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public PartyList PartyList { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public CommandManager CommandManager { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ChatGui ChatGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public GameGui GameGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public DtrBar DtrBar { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public FlyTextGui FlyTextGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public PartyFinderGui PartyFinderGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ToastGui ToastGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public LibcFunction LibcFunction { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public GameNetwork GameNetwork { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public TitleScreenMenu TitleScreenMenu { get; private set; }

        #endregion

        #region IDisposable

        /// <summary>
        ///     Divination プラグイン内で確保されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     .NET 管理リソースを解放します。
        /// </summary>
        protected virtual void ReleaseManaged()
        {
        }

        /// <summary>
        ///     .NET 管理外のリソースの解放を試みます。
        /// </summary>
        protected virtual void ReleaseUnmanaged()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;

            if (disposing)
            {
                ReleaseManaged();
                Divination.Dispose();
            }

            ReleaseUnmanaged();
        }

        ~DivinationPlugin()
        {
            Dispose(false);
        }

        #endregion
    }
}
