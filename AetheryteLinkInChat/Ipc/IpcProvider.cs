using System;
using System.Linq;
using System.Threading;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Services;
using Divination.AetheryteLinkInChat.IpcModel;
using Divination.AetheryteLinkInChat.Solver;
using Lumina.Excel.Sheets;

namespace Divination.AetheryteLinkInChat.Ipc;

public class IpcProvider : IDisposable
{
    private readonly Teleporter teleporter;
    private readonly IObjectTable objectTable;
    private readonly AetheryteSolver solver;
    private readonly IDataManager dataManager;
    private readonly ICallGateProvider<TeleportPayload, bool> teleport;
    private readonly CancellationTokenSource cancellation = new();

    public IpcProvider(IDalamudPluginInterface pluginInterface, IObjectTable objectTable, Teleporter teleporter, AetheryteSolver solver, IDataManager dataManager)
    {
        this.teleporter = teleporter;
        this.objectTable = objectTable;
        this.solver = solver;
        this.dataManager = dataManager;

        teleport = pluginInterface.GetIpcProvider<TeleportPayload, bool>(TeleportPayload.Name);
        teleport.RegisterFunc(OnTeleport);
    }

    private bool OnTeleport(TeleportPayload payload)
    {
        DalamudLog.Log.Debug("OnTeleport: {Payload}", payload);

        var world = objectTable.LocalPlayer?.CurrentWorld.Value;
        if (payload.WorldId.HasValue)
        {
            world = dataManager.GetExcelSheet<World>().GetRow(payload.WorldId.Value);
        }

        var mapLink = new MapLinkPayload(payload.TerritoryTypeId, payload.MapId, payload.Coordinates.X, payload.Coordinates.Y);
        var paths = solver.CalculateTeleportPathsForMapLink(mapLink).ToList();
        if (paths.Count == 0)
        {
            DalamudLog.Log.Debug("OnTeleport: paths.Count == 0");
            return false;
        }

        // TODO: handling cancellation

        teleporter.TeleportToPaths(paths, world, cancellation.Token).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DalamudLog.Log.Debug("OnTeleport: task.IsCompleted");
            }
            else
            {
                DalamudLog.Log.Warning(task.Exception, "OnTeleport: task.IsFaulted");
            }
        });
        return true;
    }

    public void Dispose()
    {
        teleport.UnregisterFunc();
        cancellation.Cancel();
    }
}
