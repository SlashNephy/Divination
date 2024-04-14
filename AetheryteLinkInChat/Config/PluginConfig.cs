using Dalamud.Configuration;

namespace Divination.AetheryteLinkInChat.Config;

public class PluginConfig : IPluginConfiguration
{
    public int Version { get; set; }

    public bool AllowTeleportQueueing;
    public int QueuedTeleportDelay = 3000;
    public int PreferredGrandCompanyAetheryte;
    public bool ConsiderTeleportsToOtherWorlds;
    public bool EnableLifestreamIntegration;

    public bool EnableChatNotificationOnTeleport = true;
    public bool EnableQuestNotificationOnTeleport = true;
}
