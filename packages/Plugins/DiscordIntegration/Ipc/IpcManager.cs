﻿using Dalamud.Logging;
using Dalamud.Plugin.Ipc;
using Divination.DiscordIntegration.IpcModel;

namespace Divination.DiscordIntegration.Ipc;

public static class IpcManager
{
    public static void Register()
    {
        CreateUpdateVariablesSubscriber().Subscribe(OnUpdateVariables);
        CreateClearVariablesSubscriber().Subscribe(OnClearVariables);
        CreateUpdateTemplatesSubscriber().Subscribe(OnUpdateTemplates);
        CreateClearTemplatesSubscriber().Subscribe(OnClearTemplates);
    }

    public static void Unregister()
    {
        CreateUpdateVariablesSubscriber().Unsubscribe(OnUpdateVariables);
        CreateClearVariablesSubscriber().Unsubscribe(OnClearVariables);
        CreateUpdateTemplatesSubscriber().Unsubscribe(OnUpdateTemplates);
        CreateClearTemplatesSubscriber().Unsubscribe(OnClearTemplates);
    }

    private static ICallGateSubscriber<UpdateVariablesPayload, object> CreateUpdateVariablesSubscriber()
    {
        return DiscordIntegration.Instance.Dalamud.PluginInterface.GetIpcSubscriber<UpdateVariablesPayload, object>(DiscordIntegrationIpcs.UpdateVariables);
    }

    private static void OnUpdateVariables(UpdateVariablesPayload payload)
    {
        IpcPayloadManager.ClearVariables(payload.Source, payload.Group);
        IpcPayloadManager.UpdateVariables(payload.Source, payload.Group, payload.Variables);

        PluginLog.Verbose($"UpdateVariablesPayload (from {payload.Source}, group = {payload.Group}): {payload.Variables}");
    }

    private static ICallGateSubscriber<ClearVariablesPayload, object> CreateClearVariablesSubscriber()
    {
        return DiscordIntegration.Instance.Dalamud.PluginInterface.GetIpcSubscriber<ClearVariablesPayload, object>(DiscordIntegrationIpcs.ClearVariables);
    }

    private static void OnClearVariables(ClearVariablesPayload payload)
    {
        IpcPayloadManager.ClearVariables(payload.Source, payload.Group);

        PluginLog.Verbose($"ClearVariablesPayload (from {payload.Source}, group = {payload.Group})");
    }

    private static ICallGateSubscriber<UpdateTemplatesPayload, object> CreateUpdateTemplatesSubscriber()
    {
        return DiscordIntegration.Instance.Dalamud.PluginInterface.GetIpcSubscriber<UpdateTemplatesPayload, object>(DiscordIntegrationIpcs.UpdateTemplates);
    }

    private static void OnUpdateTemplates(UpdateTemplatesPayload payload)
    {
        IpcPayloadManager.ClearTemplates(payload.Source, payload.Group);
        IpcPayloadManager.UpdateTemplates(payload.Source, payload.Group, payload.Templates);

        PluginLog.Verbose($"UpdateTemplatesPayload (from {payload.Source}, group = {payload.Group}): {payload.Templates}");
    }

    private static ICallGateSubscriber<ClearTemplatesPayload, object> CreateClearTemplatesSubscriber()
    {
        return DiscordIntegration.Instance.Dalamud.PluginInterface.GetIpcSubscriber<ClearTemplatesPayload, object>(DiscordIntegrationIpcs.ClearTemplates);
    }

    private static void OnClearTemplates(ClearTemplatesPayload payload)
    {
        IpcPayloadManager.ClearTemplates(payload.Source, payload.Group);

        PluginLog.Verbose($"ClearTemplatesPayload (from {payload.Source}, group = {payload.Group})");
    }
}
