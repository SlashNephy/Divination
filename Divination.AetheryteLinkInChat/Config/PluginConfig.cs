using Dalamud.Configuration;

namespace Divination.AetheryteLinkInChat.Config;

public class PluginConfig : IPluginConfiguration
{
    public int Version { get; set; }

    public bool PrintAetheryteName;
    public bool AllowTeleportQueueing;
    public int QueuedTeleportDelay = 2000;
}
