using System;
using System.Collections.Generic;

#nullable enable
namespace Divination.ACT.TheHuntNotifier.TheHunt.Data
{
    public class ApiResponse
    {
        public ApiResponse(List<Entry> entries)
        {
            Entries = entries;
        }

        public List<Entry> Entries { get; }

        public class Entry
        {
            public Entry(int mobId, List<Report> reports)
            {
                MobId = mobId;
                Reports = reports;
            }

            public int MobId { get; }
            public List<Report> Reports { get; }

            public Mob? Mob
            {
                get { return Array.Find(Resources.MobTable, m => m.Id == MobId); }
            }

            public IEnumerable<Report> GetReportsAt(int instance)
            {
                return Reports.FindAll(r => r.Instance == instance);
            }
        }
    }
}
