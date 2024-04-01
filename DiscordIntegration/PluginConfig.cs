using Dalamud.Configuration;

namespace Divination.DiscordIntegration;

public class PluginConfig : IPluginConfiguration
{
    public string DetailsFormat = string.Empty;
    public string DetailsInOnlineFormat = string.Empty;
    public string DetailsInDutyFormat = string.Empty;
    public string DetailsInCombatFormat = string.Empty;
    public string StateFormat = string.Empty;
    public bool ShowJobSmallImage = true;
    public string SmallImageTextFormat = string.Empty;
    public bool ShowLoadingLargeImage = true;
    public string LargeImageTextFormat = string.Empty;
    public bool ResetTimerOnAreaChange = true;
    public bool RequireTargetingOnCombat = true;

    public int Version { get; set; } = 0;
}
