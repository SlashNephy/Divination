using System;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Buddy;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.IoC;
using Dalamud.Plugin;

#pragma warning disable 8618
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Dalamud.Divination.Common.Api.Dalamud
{
    internal sealed class DalamudApi : IDalamudApi
    {
        #region IoC

        [PluginService]
        [RequiredVersion("1.0")]
        public DataManager DataManager { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ChatHandlers ChatHandlers { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public BuddyList BuddyList { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ClientState ClientState { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public Condition Condition { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public FateTable FateTable { get; private set; }

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
        public Framework Framework { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public ChatGui ChatGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public FlyTextGui FlyTextGui { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public GameGui GameGui { get; private set; }

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
        public SigScanner SigScanner { get; private set; }

        [PluginService]
        [RequiredVersion("1.0")]
        public SeStringManager SeStringManager { get; private set; }

        #endregion

        public DalamudPluginInterface PluginInterface { get; }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public DalamudApi(DalamudPluginInterface pluginInterface)
        {
            PluginInterface = pluginInterface;

            if (!pluginInterface.Inject(this))
            {
                throw new PlatformNotSupportedException("Failed to inject via IoC. Dalamud API might make breaking changes.");
            }
        }
    }
}
