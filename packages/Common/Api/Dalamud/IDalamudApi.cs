﻿using System.Diagnostics.CodeAnalysis;
using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Dalamud.Divination.Common.Api.Dalamud;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IDalamudApi
{
    public DalamudPluginInterface PluginInterface { get; }

    #region IoC

    // https://github.com/goatcorp/Dalamud/tree/master/Dalamud/Plugin/Services

    public IAddonEventManager AddonEventManager { get; }
    public IAddonLifecycle AddonLifecycle { get; }
    public IAetheryteList AetheryteList { get; }
    public IBuddyList BuddyList { get; }
    public IChatGui ChatGui { get; }
    public IClientState ClientState { get; }
    public ICommandManager CommandManager { get; }
    public ICondition Condition { get; }
    public IDataManager DataManager { get; }
    public IDtrBar DtrBar { get; }
    public IDutyState DutyState { get; }
    public IFateTable FateTable { get; }
    public IFlyTextGui FlyTextGui { get; }
    public IFramework Framework { get; }
    public IGameConfig GameConfig { get; }
    public IGameGui GameGui { get; }
    public IGameInteropProvider GameInteropProvider { get; }
    public IGameInventory GameInventory { get; }
    public IGameLifecycle GameLifecycle { get; }
    public IGameNetwork GameNetwork { get; }
    public IGamepadState GamepadState { get; }
    public IJobGauges JobGauges { get; }
    public IKeyState KeyState { get; }
    public ILibcFunction LibcFunction { get; }
    public IObjectTable ObjectTable { get; }
    public IPartyFinderGui PartyFinderGui { get; }
    public IPartyList PartyList { get; }
    public IPluginLog PluginLog { get; }
    public ISigScanner SigScanner { get; }
    public ITargetManager TargetManager { get; }
    public ITextureProvider TextureProvider { get; }
    public ITextureSubstitutionProvider TextureSubstitutionProvider { get; }
    public ITitleScreenMenu TitleScreenMenu { get; }
    public IToastGui ToastGui { get; }

    #endregion
}
