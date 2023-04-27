using System.Linq;
using Dalamud.Game.ClientState.Aetherytes;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;
using Condition = Dalamud.Game.ClientState.Conditions.Condition;

namespace Divination.AetheryteLinkInChat;

public class Teleporter
{
    private readonly ConditionFlag[] teleportUnavailableFlags =
    {
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
    };

    private Aetheryte? queuedAetheryte;
    private readonly object queuedAetheryteLock = new();
    private readonly Condition condition;
    private readonly AetheryteList aetheryteList;

    public Teleporter(Condition condition, AetheryteList aetheryteList)
    {
        this.condition = condition;
        this.aetheryteList = aetheryteList;
    }

    public bool IsTeleportUnavailable => teleportUnavailableFlags.Any(x => condition[x]);

    public unsafe bool TeleportToAetheryte(Aetheryte aetheryte)
    {
        queuedAetheryte = default;

        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            PluginLog.Debug("TeleportToAetheryte: teleport == null");
            return false;
        }

        if (!CheckAetheryte(aetheryte.RowId))
        {
            PluginLog.Error("TeleportToAetheryte: aetheryte with ID {Id} is invalid.", aetheryte.RowId);
            return false;
        }

        if (!teleport->Teleport(aetheryte.RowId, 0))
        {
            PluginLog.Error("TeleportToAetheryte: could not teleport to {Id}", aetheryte.RowId);
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
