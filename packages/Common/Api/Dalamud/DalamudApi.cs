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

    public DalamudPluginInterface PluginInterface { get; }

    #region IoC

    [PluginService]
    [RequiredVersion("1.0")]
    public IDataManager DataManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITextureProvider TextureProvider { get; private set; }

    // TODO - Find something to replace chat handlers, since its private now
    /*[PluginService]
    [RequiredVersion("1.0")]
    public ChatHandlers ChatHandlers { get; private set; }*/

    [PluginService]
    [RequiredVersion("1.0")]
    public IFramework Framework { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ISigScanner SigScanner { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IClientState ClientState { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IAetheryteList AetheryteList { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IBuddyList BuddyList { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ICondition Condition { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IFateTable FateTable { get; private set; }

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
    public IObjectTable ObjectTable { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITargetManager TargetManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IPartyList PartyList { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ICommandManager CommandManager { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IChatGui ChatGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameGui GameGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IDtrBar DtrBar { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IFlyTextGui FlyTextGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IPartyFinderGui PartyFinderGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IToastGui ToastGui { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ILibcFunction LibcFunction { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public IGameNetwork GameNetwork { get; private set; }

    [PluginService]
    [RequiredVersion("1.0")]
    public ITitleScreenMenu TitleScreenMenu { get; private set; }

    #endregion
}
