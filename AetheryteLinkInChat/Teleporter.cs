﻿using System.Linq;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;

namespace Divination.AetheryteLinkInChat;

public class Teleporter(ICondition condition, IAetheryteList aetheryteList)
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

    private Aetheryte? queuedAetheryte;
    private readonly object queuedAetheryteLock = new();

    public bool IsTeleportUnavailable => teleportUnavailableFlags.Any(x => condition[x]);

    public unsafe bool TeleportToAetheryte(Aetheryte aetheryte)
    {
        queuedAetheryte = default;

        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            DalamudLog.Log.Debug("TeleportToAetheryte: teleport == null");
            return false;
        }

        if (!CheckAetheryte(aetheryte.RowId))
        {
            DalamudLog.Log.Error("TeleportToAetheryte: aetheryte with ID {Id} is invalid.", aetheryte.RowId);
            return false;
        }

        if (!teleport->Teleport(aetheryte.RowId, 0))
        {
            DalamudLog.Log.Error("TeleportToAetheryte: could not teleport to {Id}", aetheryte.RowId);
            return false;
        }

        return true;
    }

    public Aetheryte? TeleportToQueuedAetheryte()
    {
        lock (queuedAetheryteLock)
        {
            var aetheryte = queuedAetheryte;
            if (aetheryte != default && TeleportToAetheryte(aetheryte))
            {
                return aetheryte;
            }

            return default;
        }
    }

    public void QueueTeleport(Aetheryte aetheryte)
    {
        lock (queuedAetheryteLock)
        {
            queuedAetheryte = aetheryte;
        }
    }

    private bool CheckAetheryte(uint id)
    {
        return aetheryteList.Any(x => x.AetheryteId == id);
    }
}
