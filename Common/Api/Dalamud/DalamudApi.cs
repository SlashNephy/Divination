using System;
using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

#pragma warning disable 8618
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Dalamud.Divination.Common.Api.Dalamud;

internal sealed class DalamudApi : IDalamudApi
{
    // ReSharper disable once NotNullMemberIsNotInitialized
    public DalamudApi(DalamudPluginInterface pluginInterface)
    {
        PluginInterface = pluginInterface;

        if (!pluginInterface.Inject(this))
        {
            throw new PlatformNotSupportedException(
                "Failed to inject via IoC. Dalamud API might make breaking changes.");
        }
    }

    public DalamudPluginInterface PluginInterface { get; private set; }

    #region IoC

    [PluginService]
    [RequiredVersion("1.0")]
    public IAddonEventManager AddonEventManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IAddonLifecycle AddonLifecycle { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IAetheryteList AetheryteList { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IBuddyList BuddyList { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IChatGui ChatGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IClientState ClientState { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ICommandManager CommandManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ICondition Condition { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IDataManager DataManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IDtrBar DtrBar { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IDutyState DutyState { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IFateTable FateTable { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IFlyTextGui FlyTextGui { get; private set; }
    public IFramework Framework { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameConfig GameConfig { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameGui GameGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameInteropProvider GameInteropProvider { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameInventory GameInventory { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameLifecycle GameLifecycle { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameNetwork GameNetwork { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGamepadState GamepadState { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IJobGauges JobGauges { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IKeyState KeyState { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ILibcFunction LibcFunction { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IObjectTable ObjectTable { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IPartyFinderGui PartyFinderGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IPartyList PartyList { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IPluginLog PluginLog { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ISigScanner SigScanner { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITargetManager TargetManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITextureProvider TextureProvider { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITextureSubstitutionProvider TextureSubstitutionProvider { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITitleScreenMenu TitleScreenMenu { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IToastGui ToastGui { get; private set; }

    #endregion
}
