using System.Linq;
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
    };

    private Aetheryte? queuedAetheryte;
    private readonly object queuedAetheryteLock = new();
    private readonly Condition condition;

    public Teleporter(Condition condition)
    {
        this.condition = condition;
    }

    public bool IsTeleportUnavailable => teleportUnavailableFlags.Any(x => condition[x]);

    public unsafe bool TeleportToAetheryte(Aetheryte aetheryte)
    {
        lock (queuedAetheryteLock)
        {
            queuedAetheryte = default;
        }

        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            PluginLog.Debug("TeleportToAetheryte: teleport == null");
            return false;
        }

        if (!CheckAetheryte(teleport, aetheryte.RowId))
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

    public void TeleportToQueuedAetheryte()
    {
        lock (queuedAetheryteLock)
        {
            if (queuedAetheryte == default)
            {
                return;
            }

            TeleportToAetheryte(queuedAetheryte);
        }
    }

    public void QueueTeleport(Aetheryte aetheryte)
    {
        lock (queuedAetheryteLock)
        {
            queuedAetheryte = aetheryte;
        }
    }

    private static unsafe bool CheckAetheryte(Telepo* teleport, uint id)
    {
        teleport->UpdateAetheryteList();

        for (var it = teleport->TeleportList.First; it != teleport->TeleportList.Last; it++)
        {
            if (it->AetheryteId == id)
            {
                return true;
            }
        }

        return false;
    }
}
