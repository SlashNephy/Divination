using System.Diagnostics.CodeAnalysis;
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

    public IDataManager DataManager { get; }

    public ITextureProvider TextureProvider { get; }

    // TODO - Find something to replace chat handlers, since its private now
    /*public ChatHandlers ChatHandlers { get; }*/

    public IBuddyList BuddyList { get; }

    public IClientState ClientState { get; }

    public ICondition Condition { get; }

    public IFateTable FateTable { get; }

    public IJobGauges JobGauges { get; }

    public IKeyState KeyState { get; }

    public IObjectTable ObjectTable { get; }

    public ITargetManager TargetManager { get; }

    public IPartyList PartyList { get; }

    public ICommandManager CommandManager { get; }

    public IFramework Framework { get; }

    public IChatGui ChatGui { get; }

    public IFlyTextGui FlyTextGui { get; }

    public IGameGui GameGui { get; }

    public IPartyFinderGui PartyFinderGui { get; }

    public IToastGui ToastGui { get; }

    public ILibcFunction LibcFunction { get; }

    public IGameNetwork GameNetwork { get; }

    public ISigScanner SigScanner { get; }

    public IAetheryteList AetheryteList { get; }

    public IGamepadState GamepadState { get; }

    public IDtrBar DtrBar { get; }

    public ITitleScreenMenu TitleScreenMenu { get; }

    #endregion
}
