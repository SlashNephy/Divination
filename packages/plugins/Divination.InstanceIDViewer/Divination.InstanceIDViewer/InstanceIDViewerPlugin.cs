using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;

namespace Divination.InstanceIDViewer
{
    public class InstanceIDViewerPlugin : DivinationPlugin<InstanceIDViewerPlugin>, IDalamudPlugin
    {
        private readonly NetworkListener listener;

        public InstanceIDViewerPlugin(DalamudPluginInterface pluginInterface) : base(pluginInterface)
        {
            var bar = Dalamud.DtrBar.Get("InstanceIDViewer");
            listener = new NetworkListener(Divination.Chat, bar);
            Divination.Network.AddHandler(listener);
        }

        protected override void ReleaseManaged()
        {
            Divination.Network.RemoveHandler(listener);
            listener.Dispose();
        }
    }
}
