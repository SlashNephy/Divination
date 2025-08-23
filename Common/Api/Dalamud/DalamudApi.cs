using System;
using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

#pragma warning disable 8618

namespace Dalamud.Divination.Common.Api.Dalamud;

internal sealed class DalamudApi : IDalamudApi
{
    // ReSharper disable once NotNullMemberIsNotInitialized
    public DalamudApi(IDalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;

        if (!pluginInterface.Inject(this))
        {
            throw new PlatformNotSupportedException("Failed to inject via IoC. Dalamud API might make breaking changes.");
        }
    }

    public IDalamudPluginInterface PluginInterface { get; }

    #region IoC

    [PluginService]
    public IAddonEventManager AddonEventManager { get; private set; }

    [PluginService]
    public IAddonLifecycle AddonLifecycle { get; private set; }

    [PluginService]
    public IAetheryteList AetheryteList { get; private set; }

    [PluginService]
    public IBuddyList BuddyList { get; private set; }

    [PluginService]
    public IChatGui ChatGui { get; private set; }

    [PluginService]
    public IClientState ClientState { get; private set; }

    [PluginService]
    public ICommandManager CommandManager { get; private set; }

    [PluginService]
    public ICondition Condition { get; private set; }

#pragma warning disable Dalamud001
    [PluginService]
    public IConsole Console { get; private set; }
#pragma warning restore Dalamud001

    [PluginService]
    public IContextMenu ContextMenu { get; private set; }

    [PluginService]
    public IDataManager DataManager { get; private set; }

    [PluginService]
    public IDtrBar DtrBar { get; private set; }

    [PluginService]
    public IDutyState DutyState { get; private set; }

    [PluginService]
    public IFateTable FateTable { get; private set; }

    [PluginService]
    public IFlyTextGui FlyTextGui { get; private set; }

    [PluginService]
    public IFramework Framework { get; private set; }

    [PluginService]
    public IGameConfig GameConfig { get; private set; }

    [PluginService]
    public IGameGui GameGui { get; private set; }

    [PluginService]
    public IGameInteropProvider GameInteropProvider { get; private set; }

    [PluginService]
    public IGameInventory GameInventory { get; private set; }

    [PluginService]
    public IGameLifecycle GameLifecycle { get; private set; }

    [PluginService]
    public IGamepadState GamepadState { get; private set; }

    [PluginService]
    public IJobGauges JobGauges { get; private set; }

    [PluginService]
    public IKeyState KeyState { get; private set; }

    [PluginService]
    public INotificationManager NotificationManager { get; private set; }

    [PluginService]
    public IObjectTable ObjectTable { get; private set; }

    [PluginService]
    public IPartyFinderGui PartyFinderGui { get; private set; }

    [PluginService]
    public IPartyList PartyList { get; private set; }

    [PluginService]
    public IPluginLog PluginLog { get; private set; }

    [PluginService]
    public ISigScanner SigScanner { get; private set; }

    [PluginService]
    public ITargetManager TargetManager { get; private set; }

    [PluginService]
    public ITextureProvider TextureProvider { get; private set; }

    [PluginService]
    public ITextureSubstitutionProvider TextureSubstitutionProvider { get; private set; }

    [PluginService]
    public ITitleScreenMenu TitleScreenMenu { get; private set; }

    [PluginService]
    public IToastGui ToastGui { get; private set; }

    #endregion
}
