using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Configuration;
using Dalamud.Game.Text;
using Newtonsoft.Json;

namespace Divination.FaloopIntegration.Config;

public class PluginConfig : IPluginConfiguration
{
    public const int LatestVersion = 1;
    public int Version { get; set; } = LatestVersion;

    public string FaloopUsername = string.Empty;
    public string FaloopPassword = string.Empty;

    public PerRankConfig RankS = new()
    {
        EnableSpawnReport = true,
        EnableDeathReport = true,
    };

    public PerRankConfig Fate = new();

    public bool EnableActiveMobUi;
    public bool HideActiveMobUiInDuty;
    public bool EnableSimpleReports;

    public class PerRankConfig
    {
        public int Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo);
        public Dictionary<GameExpansion, Jurisdiction> Jurisdictions = new() {
            {GameExpansion.ARelmReborn, Jurisdiction.World},
            {GameExpansion.Heavensward, Jurisdiction.World},
            {GameExpansion.Stormblood, Jurisdiction.World},
            {GameExpansion.Shadowbringers, Jurisdiction.World},
            {GameExpansion.Endwalker, Jurisdiction.World},
            {GameExpansion.Dawntrail, Jurisdiction.World},
        };
        public bool EnableSpawnReport;
        public bool EnableDeathReport;
        public bool DisableInDuty;
        public bool SkipOrphanReport;
        public bool SkipPendingReport;

        [Obsolete]
        [JsonProperty("Jurisdiction")]
        public int _Jurisdiction;
        [Obsolete]
        [JsonProperty("IncludeOceaniaDataCenter")]
        public bool _IncludeOceaniaDataCenter;
        [Obsolete]
        [JsonProperty("Expansions")]
        public Dictionary<GameExpansion, bool>? _Expansions;
    }

#pragma warning disable CS0612
    public void Migrate()
    {
        for (; Version < LatestVersion; Version++)
        {
            switch (Version)
            {
                case 0:
                    Jurisdiction[] jurisdictions = [Jurisdiction.None, Jurisdiction.World, Jurisdiction.DataCenter, Jurisdiction.Region, Jurisdiction.All];

                    foreach (var config in new PerRankConfig[] { RankS, Fate })
                    {
                        if (config._Expansions == default)
                        {
                            continue;
                        }

                        foreach (var (key, value) in config._Expansions)
                        {
                            if (value)
                            {
                                config.Jurisdictions[key] = config._IncludeOceaniaDataCenter ? Jurisdiction.Travelable : jurisdictions[config._Jurisdiction];
                            }
                            else
                            {
                                config.Jurisdictions[key] = Jurisdiction.None;
                            }
                        }
                    }

                    break;
            }
        }
    }
#pragma warning restore CS0612
}
