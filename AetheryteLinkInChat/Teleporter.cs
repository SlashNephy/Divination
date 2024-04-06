using System;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin.Services;
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
    private volatile Aetheryte? queuedAetheryte;

    public Teleporter(ICondition condition, IAetheryteList aetheryteList, IChatClient chatClient)
    {
        this.condition = condition;
        this.aetheryteList = aetheryteList;
        this.chatClient = chatClient;

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

    public void Dispose()
    {
        condition.ConditionChange -= OnConditionChanged;
    }
}
