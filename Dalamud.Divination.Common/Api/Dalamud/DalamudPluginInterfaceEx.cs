using System.Collections.Generic;
using System.Reflection;
using Dalamud.Data;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Dalamud
{
    public static class DalamudPluginInterfaceEx
    {
        public static global::Dalamud.Dalamud GetDalamud(this DalamudPluginInterface @interface)
        {
            var field = typeof(DalamudPluginInterface)
                .GetField("dalamud", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

            return (global::Dalamud.Dalamud) field!.GetValue(@interface);
        }

        public static object GetDalamudPluginManager(this DalamudPluginInterface @interface)
        {
            var property = typeof(global::Dalamud.Dalamud)
                .GetProperty("PluginManager", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

            return property!.GetValue(@interface.GetDalamud());
        }

        public static IEnumerable<(IDalamudPlugin Plugin, PluginDefinition Definition, DalamudPluginInterface PluginInterface, bool IsRaw)> GetDalamudPlugins(this DalamudPluginInterface @interface)
        {
            var manager = @interface.GetDalamudPluginManager();
            var field = manager.GetType().GetField("Plugins");

            return (List<(IDalamudPlugin, PluginDefinition, DalamudPluginInterface, bool)>) field.GetValue(manager);
        }

        public static Lumina.GameData GetLumina(this DalamudPluginInterface @interface)
        {
            var field = typeof(DataManager)
                .GetField("gameData", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

            return (Lumina.GameData) field!.GetValue(@interface.Data);
        }

        public static bool IsLoggedIn(this DalamudPluginInterface @interface)
        {
            return @interface.Data.IsDataReady && @interface.ClientState.TerritoryType > 0 && @interface.ClientState.Condition.Any() && @interface.ClientState.IsLoggedIn;
        }
    }
}
