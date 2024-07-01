using System;
using System.Linq;
using System.Numerics;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;
using Divination.AetheryteLinkInChat.IpcModel;

namespace Divination.FaloopIntegration.Ipc;

public class AetheryteLinkInChatIpc(IDalamudPluginInterface pluginInterface, IChatClient chatClient)
{
    private readonly ICallGateSubscriber<TeleportPayload, bool> subscriber = pluginInterface.GetIpcSubscriber<TeleportPayload, bool>(TeleportPayload.Name);

    public bool Teleport(uint territoryTypeId, uint mapId, Vector2 coordinates, uint worldId)
    {
        if (!IsPluginInstalled())
        {
            chatClient.PrintError(Localization.AetheryteLinkInChatPluginNotInstalled);
            return false;
        }

        var payload = new TeleportPayload()
        {
            TerritoryTypeId = territoryTypeId,
            MapId = mapId,
            Coordinates = coordinates,
            WorldId = worldId,
        };

        try
        {
            return subscriber.InvokeFunc(payload);
        }
        catch (Exception e)
        {
            DalamudLog.Log.Error(e, "failed to invoke Teleport");
            return false;
        }
    }

    private bool IsPluginInstalled()
    {
        return pluginInterface.InstalledPlugins.Any(x => x.Name == "Divination.AetheryteLinkInChat" && x.IsLoaded);
    }
}
