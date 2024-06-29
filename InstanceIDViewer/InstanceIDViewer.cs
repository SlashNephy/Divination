using Dalamud.Divination.Common.Boilerplate;
using Dalamud.Plugin;

namespace Divination.InstanceIDViewer;

public class InstanceIDViewer : DivinationPlugin<InstanceIDViewer>, IDalamudPlugin
{
    private readonly NetworkListener listener;

    public InstanceIDViewer(DalamudPluginInterface pluginInterface) : base(pluginInterface)
    {
        var bar = Dalamud.DtrBar.Get("InstanceIDViewer");
        listener = new NetworkListener(Divination.Chat, bar);
        Divination.Network.AddHandler(listener);
    }

    protected override void ReleaseManaged()
    {
        Divination.Network.RemoveHandler(listener);
    }
}
