using Dalamud.Plugin.Ipc;
using Divination.DiscordIntegration.IpcModel;

namespace Divination.DiscordIntegration.Ipc
{
    public static class IpcManager
    {
        private const string UpdateVariables = "Divination.DiscordIntegration_UpdateVariables";
        private const string ClearVariables = "Divination.DiscordIntegration_ClearVariables";
        private const string UpdateTemplates = "Divination.DiscordIntegration_UpdateTemplates";
        private const string ClearTemplates = "Divination.DiscordIntegration_ClearTemplates";

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
            return DiscordIntegrationPlugin.Instance.Dalamud.PluginInterface.GetIpcSubscriber<UpdateVariablesPayload, object>(UpdateVariables);
        }

        private static void OnUpdateVariables(UpdateVariablesPayload payload)
        {
            IpcPayloadManager.ClearVariables(payload.Source, payload.Group);
            IpcPayloadManager.UpdateVariables(payload.Source, payload.Group, payload.Variables);
        }

        private static ICallGateSubscriber<ClearVariablesPayload, object> CreateClearVariablesSubscriber()
        {
            return DiscordIntegrationPlugin.Instance.Dalamud.PluginInterface.GetIpcSubscriber<ClearVariablesPayload, object>(ClearVariables);
        }

        private static void OnClearVariables(ClearVariablesPayload payload)
        {
            IpcPayloadManager.ClearVariables(payload.Source, payload.Group);
        }

        private static ICallGateSubscriber<UpdateTemplatesPayload, object> CreateUpdateTemplatesSubscriber()
        {
            return DiscordIntegrationPlugin.Instance.Dalamud.PluginInterface.GetIpcSubscriber<UpdateTemplatesPayload, object>(UpdateTemplates);
        }

        private static void OnUpdateTemplates(UpdateTemplatesPayload payload)
        {
            IpcPayloadManager.ClearTemplates(payload.Source, payload.Group);
            IpcPayloadManager.UpdateTemplates(payload.Source, payload.Group, payload.Templates);
        }

        private static ICallGateSubscriber<ClearTemplatesPayload, object> CreateClearTemplatesSubscriber()
        {
            return DiscordIntegrationPlugin.Instance.Dalamud.PluginInterface.GetIpcSubscriber<ClearTemplatesPayload, object>(ClearTemplates);
        }

        private static void OnClearTemplates(ClearTemplatesPayload payload)
        {
            IpcPayloadManager.ClearTemplates(payload.Source, payload.Group);
        }
    }
}
