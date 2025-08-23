using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Divination.Common.Api.Chat;
using Dalamud.Divination.Common.Api.Dalamud;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Divination.AetheryteLinkInChat.Config;
using Divination.AetheryteLinkInChat.Solver;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.Sheets;

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
        ConditionFlag.ExecutingGatheringAction,
        ConditionFlag.Crafting,
        ConditionFlag.ExecutingCraftingAction,
        ConditionFlag.PreparingToCraft,
    ];

    private readonly ICondition condition;
    private readonly IAetheryteList aetheryteList;
    private readonly IChatClient chatClient;
    private readonly ICommandManager commandManager;
    private readonly IClientState clientState;
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly IToastGui toastGui;
    private readonly IFramework framework;
    private readonly PluginConfig config;
    // Huh, cant use volatile here anymore... well hope nothing explodes :)
    private Aetheryte? queuedAetheryte;

    public Teleporter(ICondition condition, IAetheryteList aetheryteList, IChatClient chatClient, ICommandManager commandManager, IClientState clientState, IDalamudPluginInterface pluginInterface, IToastGui toastGui, IFramework framework, PluginConfig config)
    {
        this.condition = condition;
        this.aetheryteList = aetheryteList;
        this.chatClient = chatClient;
        this.commandManager = commandManager;
        this.clientState = clientState;
        this.pluginInterface = pluginInterface;
        this.toastGui = toastGui;
        this.framework = framework;
        this.config = config;

        condition.ConditionChange += OnConditionChanged;
    }

    public bool IsTeleportUnavailable => teleportUnavailableFlags.Any(x => condition[x]);

    public async Task<bool> TeleportToAetheryte(Aetheryte aetheryte)
    {
        if (IsTeleportUnavailable)
        {
            if (QueueTeleport(aetheryte))
            {
                DisplayQueueTeleportingNotification(aetheryte.PlaceName.Value.Name.ExtractText());
                return true;
            }

            return false;
        }

        queuedAetheryte = default;
        if (await ExecuteTeleport(aetheryte))
        {
            DisplayTeleportingNotification(aetheryte.PlaceName.Value.Name.ExtractText(), false);
            return true;
        }

        return false;
    }

    private bool QueueTeleport(Aetheryte aetheryte)
    {
        if (!config.AllowTeleportQueueing)
        {
            return false;
        }

        queuedAetheryte = aetheryte;
        return true;
    }

    private unsafe Task<bool> ExecuteTeleport(Aetheryte aetheryte)
    {
        return framework.RunOnFrameworkThread(() => ExecuteTeleportInternal(aetheryte));
    }

    private unsafe bool ExecuteTeleportInternal(Aetheryte aetheryte)
    {
        // https://github.com/NightmareXIV/Lifestream/blob/7ad417ac028ae2e2a42e61d1883fdeb9895bc128/Lifestream/Services/TeleportService.cs#L13
        var actionManager = ActionManager.Instance();
        if (actionManager == default)
        {
            DalamudLog.Log.Warning("ActionManager is null");
            return false;
        }

        if (actionManager->GetActionStatus(ActionType.Action, 5) != 0)
        {
            DalamudLog.Log.Warning("Can't execute teleport action");
            return false;
        }

        if (actionManager->AnimationLock > 0)
        {
            DalamudLog.Log.Warning("Can't teleport - animation locked");
            return false;
        }

        var teleport = Telepo.Instance();
        if (teleport == default)
        {
            DalamudLog.Log.Debug("ExecuteTeleport: teleport == null");
            return false;
        }

        foreach (var entry in aetheryteList)
        {
            if (entry.AetheryteId == aetheryte.RowId)
            {
                return teleport->Teleport(aetheryte.RowId, 0);
            }
        }

        DalamudLog.Log.Error("ExecuteTeleport: aetheryte with ID {Id} is invalid.", aetheryte.RowId);
        return false;
    }

    private void TeleportToQueuedAetheryte()
    {
        var aetheryte = queuedAetheryte;
        queuedAetheryte = default;
        if (aetheryte.HasValue)
        {
            ExecuteTeleport(aetheryte.Value).ContinueWith(_ =>
            {
                DisplayTeleportingNotification(aetheryte.Value.PlaceName.Value.Name.ExtractText(), true);
            });
        }
    }

    private void OnConditionChanged(ConditionFlag flag, bool value)
    {
        if (!config.AllowTeleportQueueing || IsTeleportUnavailable)
        {
            return;
        }

        Task.Delay(config.QueuedTeleportDelay)
            .ContinueWith(_ =>
            {
                TeleportToQueuedAetheryte();
            });
    }

    public async Task<bool> TeleportToPaths(IEnumerable<ITeleportPath> paths, World? world, CancellationToken cancellationToken)
    {
        return await framework.Run(async () => await TeleportToPathsInternal(paths, world, cancellationToken), cancellationToken: cancellationToken);
    }

    private async Task<bool> TeleportToPathsInternal(IEnumerable<ITeleportPath> paths, World? world, CancellationToken cancellationToken)
    {
        while (IsTeleportUnavailable)
        {
            await Task.Delay(500, cancellationToken);
        }

        if (world.HasValue)
        {
            if (world.Value.RowId == clientState.LocalPlayer?.CurrentWorld.RowId)
            {
                DalamudLog.Log.Debug("TeleportToPaths: world == currentWorld");
            }
            else
            {
                DalamudLog.Log.Debug("TeleportToPaths: teleporting to {World}", world.Value.Name.ExtractText());

                if (!TeleportToWorld(world.Value))
                {
                    DalamudLog.Log.Warning("TeleportToPaths: teleport to {World} failed", world.Value.Name.ExtractText());
                    return false;
                }

                DisplayTeleportingNotification(world.Value.Name.ExtractText(), false);
                DalamudLog.Log.Debug("TeleportToPaths: waiting for {World}", world.Value.Name.ExtractText());

                // wait until world changed
                while (world.Value.RowId != clientState.LocalPlayer?.CurrentWorld.RowId || IsTeleportUnavailable)
                {
                    await Task.Delay(500, cancellationToken);
                }

                DalamudLog.Log.Debug("TeleportToPaths: world changed: {World}", world.Value.Name.ExtractText());
            }
        }

        foreach (var path in paths)
        {
            while (IsTeleportUnavailable)
            {
                await Task.Delay(500, cancellationToken);
            }

            await Task.Delay(config.QueuedTeleportDelay, cancellationToken);

            switch (path)
            {
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: true } aetheryte:
                    if (!ExecuteTeleportInternal(aetheryte.Aetheryte))
                    {
                        return false;
                    }

                    DisplayTeleportingNotification(aetheryte.Aetheryte.PlaceName.Value.Name.ExtractText(), false);
                    break;
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: false } aetheryte:
                    if (!TeleportToAethernet(aetheryte.Aetheryte))
                    {
                        DalamudLog.Log.Debug("TeleportToAethernet: failed.");
                        return false;
                    }

                    DisplayTeleportingNotification(aetheryte.Aetheryte.AethernetName.Value.Name.ExtractText(), false);
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
        return commandManager.ProcessCommand($"/li {aetheryte.AethernetName.Value.Name.ExtractText()}");
    }

    private bool TeleportToWorld(World world)
    {
        return commandManager.ProcessCommand($"/li {world.Name.ExtractText()}");
    }

    public bool IsLifestreamAvailable()
    {
        return pluginInterface.InstalledPlugins.Any(x => x.InternalName == "Lifestream" && x.IsLoaded);
    }

    private void DisplayTeleportingNotification(string? destination, bool isQueued)
    {
        if (string.IsNullOrEmpty(destination))
        {
            return;
        }

        if (config.EnableChatNotificationOnTeleport)
        {
            var message = (isQueued ? Localization.QueuedTeleportingMessage : Localization.TeleportingMessage).Format(destination);
            chatClient.Print(message);
        }
        if (config.EnableQuestNotificationOnTeleport)
        {
            var message = (isQueued ? Localization.QueuedTeleportingQuestMessage : Localization.TeleportingQuestMessage).Format(destination);
            toastGui.ShowQuest(message, new Dalamud.Game.Gui.Toast.QuestToastOptions()
            {
                // Teleport action icon
                IconId = 111,
            });
        }
    }

    private void DisplayQueueTeleportingNotification(string? destination)
    {
        if (string.IsNullOrEmpty(destination))
        {
            return;
        }

        if (config.EnableChatNotificationOnTeleport)
        {
            chatClient.Print(Localization.QueueTeleportMessage.Format(destination));
        }
    }

    public void Dispose()
    {
        condition.ConditionChange -= OnConditionChanged;
    }
}
