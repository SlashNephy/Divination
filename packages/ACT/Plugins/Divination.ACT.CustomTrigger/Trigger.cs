using System;
using System.Text.RegularExpressions;
using Advanced_Combat_Tracker;

namespace Divination.ACT.CustomTrigger
{
    public class Trigger
    {
        public Trigger(Regex regex, Action<Event> action)
        {
            Regex = regex;
            Action = action;
        }

        public Regex Regex { get; }
        public Action<Event> Action { get; }

        public class Event
        {
            public Event(LogLineEventArgs logInfo, Match match)
            {
                LogLine = logInfo;
                MatchResult = match;
            }

            public LogLineEventArgs LogLine { get; }
            public Match MatchResult { get; }

            public string MatchedGroup => MatchResult.Groups[1].Value;
            public (string, string) MatchedGroups2 => (MatchResult.Groups[1].Value, MatchResult.Groups[2].Value);
        }
    }
}
