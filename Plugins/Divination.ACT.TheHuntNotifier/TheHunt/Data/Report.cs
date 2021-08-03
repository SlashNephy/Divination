using System;

#nullable enable
namespace Divination.ACT.TheHuntNotifier.TheHunt.Data
{
    public class Report
    {
        public Report(Guid id, Guid uid, int instance, DateTime time, Point? point, int score)
        {
            Id = id;
            UserId = uid;
            Instance = instance;
            Time = time;
            Point = point;
            Score = score;
        }

        public Guid Id { get; }
        public Guid UserId { get; }
        public int Instance { get; }
        public DateTime Time { get; }
        public Point? Point { get; }
        public int Score { get; }

        public bool IsTrusted => Score >= 1000;
    }
}
