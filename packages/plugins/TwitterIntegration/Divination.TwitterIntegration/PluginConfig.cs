using Dalamud.Configuration;

namespace Divination.TwitterIntegration;

public class PluginConfig : IPluginConfiguration
{
    public string ConsumerKey = string.Empty;
    public string ConsumerSecret = string.Empty;
    public string AccessToken = string.Empty;
    public string AccessTokenSecret = string.Empty;

    public bool ShowListTimeline = false;
    public string ListId = string.Empty;
    public int UpdateIntervalInMs = 3000;
    public long? SinceId = null;

    public int Version { get; set; } = 0;
}
