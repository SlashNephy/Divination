using System;

#nullable enable
namespace Divination.ACT.TheHuntNotifier.TheHunt.Data
{
    public class Mob
    {
        public enum MobRank
        {
            A,
            S,
            F
        }

        public Mob(int id, int areaId, MobRank rank, int? interval, string name)
        {
            Id = id;
            AreaId = areaId;
            Rank = rank;
            Interval = interval;
            Name = name;
        }

        public int Id { get; }
        public int AreaId { get; }
        public MobRank Rank { get; }
        public int? Interval { get; }
        public string Name { get; }

        public Area Area
        {
            get { return Array.Find(Resources.AreaTable, a => a.Id == AreaId); }
        }

        public bool HasTimer => Interval != null;
    }
}
