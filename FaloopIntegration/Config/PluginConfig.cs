using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Configuration;
using Dalamud.Game.Text;

namespace Divination.FaloopIntegration.Config;

public class PluginConfig : IPluginConfiguration
{
    public int Version { get; set; }

    public string FaloopUsername = string.Empty;
    public string FaloopPassword = string.Empty;

    public PerRankConfig RankS = new()
    {
        Jurisdiction = (int)Jurisdiction.World,
        EnableSpawnReport = true,
        EnableDeathReport = true,
    };

    public PerRankConfig Fate = new();

    public List<MobSpawnEvent> SpawnStates = [];
    public bool EnableActiveMobUi;
    public bool EnableSimpleReports;

    public class PerRankConfig
    {
        public int Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo);
        public int Jurisdiction;
        public bool IncludeOceaniaDataCenter;
        public Dictionary<GameExpansion, bool> Expansions = new()
        {
            {GameExpansion.ARelmReborn, true},
            {GameExpansion.Heavensward, true},
            {GameExpansion.Stormblood, true},
            {GameExpansion.Shadowbringers, true},
            {GameExpansion.Endwalker, true},
            {GameExpansion.Dawntrail, true},
        };
        public bool EnableSpawnReport;
        public bool EnableDeathReport;
        public bool DisableInDuty;
        public bool SkipOrphanReport;
        public bool SkipPendingReport;
    }
}
