using System;
using System.Collections.Generic;
using Divination.ACT.TheHuntNotifier.TheHunt.Data;

namespace Divination.ACT.TheHuntNotifier
{
    public class Settings : PluginSettings
    {
        public Settings()
        {
            this.Bind("World", Plugin.TabControl.worldComboBox);
            this.Bind("NotifyRankS", Plugin.TabControl.rankSCheckBox);
            this.Bind("NotifyRankA", Plugin.TabControl.rankACheckBox);
            this.Bind("NotifyRankF", Plugin.TabControl.rankFCheckBox);
            this.Bind("Interval", Plugin.TabControl.intervalTextBox);
        }

        public static int IntervalMs => int.TryParse(Plugin.TabControl.intervalTextBox.Text, out var interval)
            ? interval * 1000
            : 30000;

        public static World World =>
            Enum.IsDefined(typeof(World), Plugin.TabControl.worldComboBox.Text)
                ? (World) Enum.Parse(typeof(World), Plugin.TabControl.worldComboBox.Text)
                : World.Valefor;

        public static List<Mob.MobRank> TargetMobRanks
        {
            get
            {
                var ranks = new List<Mob.MobRank>();
                if (Plugin.TabControl.rankSCheckBox.Checked)
                {
                    ranks.Add(Mob.MobRank.S);
                }

                if (Plugin.TabControl.rankACheckBox.Checked)
                {
                    ranks.Add(Mob.MobRank.A);
                }

                if (Plugin.TabControl.rankFCheckBox.Checked)
                {
                    ranks.Add(Mob.MobRank.F);
                }

                return ranks;
            }
        }
    }
}
