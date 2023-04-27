using Dalamud.Configuration;

namespace Divination.ChatFilter;

public class PluginConfig : IPluginConfiguration
{
    public bool FilterLsGreetingMessages = true;
    public bool FilterMobHuntShoutMessages = true;
    public bool EnableDuplicatedMessageFilter = true;
    public bool EnableNoticeMessageFilter = false;
    public bool EnableNoviceNetworkJoinMessagesFilter = false;
    public bool EnableDebugMessageFilter = false;
    public bool MoveDebugMessagesToEcho = false;
    public bool EnableSonarRankBFilter = false;
    public bool FilterEchoDuplication = false;
    public bool FilterOldMobKills = true;
    public bool FilterSameMapLinks = false;

    public int Version { get; set; }
}