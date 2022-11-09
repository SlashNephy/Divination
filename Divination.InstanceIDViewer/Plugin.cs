using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;

namespace Divination.InstanceIDViewer
{
    public class Plugin : DivinationPlugin<Plugin>, IDalamudPlugin
    {
        private readonly NetworkListener listener;

        public Plugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            listener = new NetworkListener(Divination.Chat);
            Divination.Network.AddHandler(listener);
        }

        protected override void ReleaseManaged()
        {
            Divination.Network.RemoveHandler(listener);
        }
    }
}
