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
        Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo),
        Jurisdiction = (int)Jurisdiction.World,
        EnableSpawnReport = true,
        EnableDeathReport = true,
    };

    public PerRankConfig RankA = new()
    {
        Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo),
    };

    public PerRankConfig RankB = new()
    {
        Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo),
    };

    public PerRankConfig Fate = new()
    {
        Channel = Enum.GetValues<XivChatType>().ToList().IndexOf(XivChatType.Echo),
    };

    public class PerRankConfig
    {
        public int Channel;
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
    }
}
