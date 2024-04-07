using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin.Services;
using Divination.AetheryteLinkInChat.Solver;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public sealed class Teleporter : IDisposable
{
    private readonly ConditionFlag[] teleportUnavailableFlags =
    [
        ConditionFlag.InCombat,
        ConditionFlag.Casting,
        ConditionFlag.BetweenAreas,
        ConditionFlag.BetweenAreas51,
        ConditionFlag.WaitingToVisitOtherWorld,
        ConditionFlag.ReadyingVisitOtherWorld,
        ConditionFlag.OccupiedInEvent,
        ConditionFlag.OccupiedInQuestEvent,
        ConditionFlag.OccupiedSummoningBell,
        ConditionFlag.OccupiedInCutSceneEvent,
        ConditionFlag.Occupied33,
        ConditionFlag.BoundByDuty,
        ConditionFlag.BoundByDuty56,
        ConditionFlag.Fishing,
        ConditionFlag.Gathering,
        ConditionFlag.Gathering42,
        ConditionFlag.Crafting,
        ConditionFlag.Crafting40,
        ConditionFlag.PreparingToCraft,
    ];

    private readonly ICondition condition;
    private readonly IAetheryteList aetheryteList;
    private readonly IChatClient chatClient;
    private readonly ICommandManager commandManager;
    private readonly IClientState clientState;
    private volatile Aetheryte? queuedAetheryte;

    public Teleporter(ICondition condition, IAetheryteList aetheryteList, IChatClient chatClient, ICommandManager commandManager, IClientState clientState)
    {
        this.condition = condition;
        this.aetheryteList = aetheryteList;
        this.chatClient = chatClient;
        this.commandManager = commandManager;
        this.clientState = clientState;

        condition.ConditionChange += OnConditionChanged;
    }

    public bool IsTeleportUnavailable => teleportUnavailableFlags.Any(x => condition[x]);

    public unsafe bool TeleportToAetheryte(Aetheryte aetheryte)
    {
        if (IsTeleportUnavailable)
        {
            if (QueueTeleport(aetheryte))
            {
                chatClient.Print(Localization.QueueTeleportMessage.Format(aetheryte.PlaceName.Value?.Name.RawString));
                return true;
            }

            return false;
        }

        queuedAetheryte = default;
        if (ExecuteTeleport(aetheryte))
        {
            chatClient.Print(Localization.TeleportingMessage.Format(aetheryte.PlaceName.Value?.Name.RawString));
            return true;
        }

        return false;
    }

    private bool QueueTeleport(Aetheryte aetheryte)
    {
        if (!AetheryteLinkInChat.Instance.Config.AllowTeleportQueueing)
        {
            return false;
        }

        queuedAetheryte = aetheryte;
        return true;
    }

    private unsafe bool ExecuteTeleport(Aetheryte aetheryte)
    {
        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            DalamudLog.Log.Debug("ExecuteTeleport: teleport == null");
            return false;
        }

        if (!aetheryteList.Any(x => x.AetheryteId == aetheryte.RowId))
        {
            DalamudLog.Log.Error("ExecuteTeleport: aetheryte with ID {Id} is invalid.", aetheryte.RowId);
            return false;
        }

        if (!teleport->Teleport(aetheryte.RowId, 0))
        {
            DalamudLog.Log.Error("ExecuteTeleport: could not teleport to {Id}", aetheryte.RowId);
            return false;
        }

        return true;
    }

    private void TeleportToQueuedAetheryte()
    {

        var aetheryte = queuedAetheryte;
        queuedAetheryte = default;
        if (aetheryte != default && ExecuteTeleport(aetheryte))
        {
            chatClient.Print(Localization.QueuedTeleportingMessage.Format(aetheryte.PlaceName.Value?.Name.RawString));
            return;
        }
    }

    private void OnConditionChanged(ConditionFlag flag, bool value)
    {
        if (!AetheryteLinkInChat.Instance.Config.AllowTeleportQueueing || IsTeleportUnavailable)
        {
            return;
        }

        Task.Delay(AetheryteLinkInChat.Instance.Config.QueuedTeleportDelay)
            .ContinueWith(_ =>
            {
                TeleportToQueuedAetheryte();
            });
    }

    public async Task<bool> TeleportToPaths(IEnumerable<ITeleportPath> paths, World? world, CancellationToken cancellationToken)
    {
        if (world != default)
        {
            if (world.RowId == clientState.LocalPlayer?.CurrentWorld?.Id)
            {
                DalamudLog.Log.Debug("TeleportToPaths: world == currentWorld");
            }
            else
            {
                DalamudLog.Log.Debug("TeleportToPaths: teleporting to {World}", world.Name.RawString);

                if (!TeleportToWorld(world))
                {
                    DalamudLog.Log.Warning("TeleportToPaths: teleport to {World} failed", world.Name.RawString);
                    return false;
                }

                chatClient.Print(Localization.TeleportingMessage.Format(world.Name.RawString));
                DalamudLog.Log.Debug("TeleportToPaths: waiting for {World}", world.Name.RawString);

                // wait until world changed
                while (world.RowId != clientState.LocalPlayer?.CurrentWorld.Id)
                {
                    await Task.Delay(100, cancellationToken);
                }

                DalamudLog.Log.Debug("TeleportToPaths: world changed: {World}", world.Name.RawString);
            }
        }

        foreach (var path in paths)
        {
            while (IsTeleportUnavailable)
            {
                await Task.Delay(100, cancellationToken);
            }

            switch (path)
            {
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: true } aetheryte:
                    if (!ExecuteTeleport(aetheryte.Aetheryte))
                    {
                        return false;
                    }

                    chatClient.Print(Localization.TeleportingMessage.Format(aetheryte.Aetheryte.PlaceName.Value?.Name.RawString));
                    break;
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: false } aetheryte:
                    if (!TeleportToAethernet(aetheryte.Aetheryte))
                    {
                        DalamudLog.Log.Debug("TeleportToAethernet: failed.");
                        return false;
                    }

                    chatClient.Print(Localization.TeleportingMessage.Format(aetheryte.Aetheryte.AethernetName.Value?.Name.RawString));
                    break;
                case BoundaryTeleportPath boundary:
                    throw new NotImplementedException("boundary teleport is not implemented.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(path), path, "unknown path type.");
            }
        }

        return true;
    }

    private bool TeleportToAethernet(Aetheryte aetheryte)
    {
        return commandManager.ProcessCommand($"/li {aetheryte.AethernetName.Value?.Name.RawString}");
    }

    private bool TeleportToWorld(World world)
    {
        return commandManager.ProcessCommand($"/li {world.Name.RawString}");
    }

    public void Dispose()
    {
        condition.ConditionChange -= OnConditionChanged;
    }
}
