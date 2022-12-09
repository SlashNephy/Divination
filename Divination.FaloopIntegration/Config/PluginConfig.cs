using Dalamud.Configuration;
using Dalamud.Game.Text;

namespace Divination.FaloopIntegration.Config;

public class PluginConfig : IPluginConfiguration
{
    public int Version { get; set; }

    public string FaloopUsername = string.Empty;
    public string FaloopPassword = string.Empty;

    public int RankSJurisdiction = (int) Jurisdiction.World;
    public int RankAJurisdiction;
    public int RankBJurisdiction;
    public int FateJurisdiction;

    public int Channel = (int) XivChatType.Echo;
    public bool EnableSpawnReport;
    public bool EnableDeathReport;
}
