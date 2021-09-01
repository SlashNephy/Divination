using System.Collections.Generic;
using System.Reflection;
using Dalamud.Data;
using Dalamud.Game.ClientState;
using Dalamud.Plugin;

namespace Dalamud.Divination.Common.Api.Dalamud
{
    public static class DalamudPluginInterfaceEx
    {
        public static object GetDalamud(this DalamudPluginInterface @interface)
        {
            var field = typeof(DalamudPluginInterface)
                .GetField("dalamud", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

            return field!.GetValue(@interface)!;
        }
    }
}
