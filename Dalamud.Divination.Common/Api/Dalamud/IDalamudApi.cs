using System.Diagnostics.CodeAnalysis;
using Dalamud.Data;
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
using Dalamud.Game.Gui.ContextMenus;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Interface;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Dalamud
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IDalamudApi
    {
        #region IoC

        public DataManager DataManager { get; }

        public ChatHandlers ChatHandlers { get; }

        public BuddyList BuddyList { get; }

        public ClientState ClientState { get; }

        public Condition Condition { get; }

        public FateTable FateTable { get; }

        public JobGauges JobGauges { get; }

        public KeyState KeyState { get; }

        public ObjectTable ObjectTable { get; }

        public TargetManager TargetManager { get; }

        public PartyList PartyList { get; }

        public CommandManager CommandManager { get; }

        public Framework Framework { get; }

        public ChatGui ChatGui { get; }

        public FlyTextGui FlyTextGui { get; }

        public GameGui GameGui { get; }

        public PartyFinderGui PartyFinderGui { get; }

        public ToastGui ToastGui { get; }

        public LibcFunction LibcFunction { get; }

        public GameNetwork GameNetwork { get; }

        public SigScanner SigScanner { get; }

        public AetheryteList AetheryteList { get; }

        public GamepadState GamepadState { get; }

        public ContextMenu ContextMenu { get; }

        public DtrBar DtrBar { get; }

        public TitleScreenMenu TitleScreenMenu { get; }

        #endregion

        public DalamudPluginInterface PluginInterface { get; }
    }
}
