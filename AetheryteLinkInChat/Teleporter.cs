﻿using System;
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
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly IToastGui toastGui;
    private readonly PluginConfig config;
    private volatile Aetheryte? queuedAetheryte;

    public Teleporter(ICondition condition, IAetheryteList aetheryteList, IChatClient chatClient, ICommandManager commandManager, IClientState clientState, IDalamudPluginInterface pluginInterface, IToastGui toastGui, PluginConfig config)
    {
        this.condition = condition;
        this.aetheryteList = aetheryteList;
        this.chatClient = chatClient;
        this.commandManager = commandManager;
        this.clientState = clientState;
        this.pluginInterface = pluginInterface;
        this.toastGui = toastGui;
        this.config = config;

        condition.ConditionChange += OnConditionChanged;
    }

    public bool IsTeleportUnavailable => teleportUnavailableFlags.Any(x => condition[x]);

    public unsafe bool TeleportToAetheryte(Aetheryte aetheryte)
    {
        if (IsTeleportUnavailable)
        {
            if (QueueTeleport(aetheryte))
            {
                DisplayQueueTeleportingNotification(aetheryte.PlaceName.Value?.Name.RawString);
                return true;
            }

            return false;
        }

        queuedAetheryte = default;
        if (ExecuteTeleport(aetheryte))
        {
            DisplayTeleportingNotification(aetheryte.PlaceName.Value?.Name.RawString, false);
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
            DisplayTeleportingNotification(aetheryte.PlaceName.Value?.Name.RawString, true);
            return;
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
        while (IsTeleportUnavailable)
        {
            await Task.Delay(500, cancellationToken);
        }

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

                DisplayTeleportingNotification(world.Name.RawString, false);
                DalamudLog.Log.Debug("TeleportToPaths: waiting for {World}", world.Name.RawString);

                // wait until world changed
                while (world.RowId != clientState.LocalPlayer?.CurrentWorld.Id || IsTeleportUnavailable)
                {
                    await Task.Delay(500, cancellationToken);
                }

                DalamudLog.Log.Debug("TeleportToPaths: world changed: {World}", world.Name.RawString);
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
                    if (!ExecuteTeleport(aetheryte.Aetheryte))
                    {
                        return false;
                    }

                    DisplayTeleportingNotification(aetheryte.Aetheryte.PlaceName.Value?.Name.RawString, false);
                    break;
                case AetheryteTeleportPath { Aetheryte.IsAetheryte: false } aetheryte:
                    if (!TeleportToAethernet(aetheryte.Aetheryte))
                    {
                        DalamudLog.Log.Debug("TeleportToAethernet: failed.");
                        return false;
                    }

                    DisplayTeleportingNotification(aetheryte.Aetheryte.AethernetName.Value?.Name.RawString, false);
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
