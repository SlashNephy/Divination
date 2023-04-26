#nullable enable
namespace Divination.ACT.TheHuntNotifier.TheHunt.Data
{
    public class Area
    {
        public Area(int id, int? instance, string name)
        {
            Id = id;
            Instance = instance ?? 1;
            Name = name;
        }

        public int Id { get; }
        public int Instance { get; }
        public string Name { get; }

        public bool HasInstance => Instance > 1;
    }
}
