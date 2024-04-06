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

    public PerRankConfig RankA = new();

    public PerRankConfig RankB = new();

    public PerRankConfig Fate = new();

    public List<SpawnHistory> SpawnHistories = [];
    public bool EnableActiveMobUi = false;
    public bool EnableSimpleReports = false;

    public class PerRankConfig
    {
        public int Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo);
        public int Jurisdiction;
        public Dictionary<MajorPatch, bool> MajorPatches = new()
        {
            {MajorPatch.ARealmReborn, true},
            {MajorPatch.Heavensward, true},
            {MajorPatch.Stormblood, true},
            {MajorPatch.Shadowbringer, true},
            {MajorPatch.Endwalker, true},
        };
        public bool EnableSpawnReport;
        public bool EnableDeathReport;
        public bool DisableInDuty;
        public bool SkipOrphanReport;
    }

    public class SpawnHistory
    {
        public uint MobId;
        public uint WorldId;
        public DateTime At;
    }
}
