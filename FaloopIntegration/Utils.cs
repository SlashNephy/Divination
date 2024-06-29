using System;
using System.Numerics;
using System.Text;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;

namespace Divination.FaloopIntegration;

public static class Utils
{
    public static SeIconChar GetRankIconChar(MobRank rank)
    {
        return rank switch
        {
            MobRank.B => SeIconChar.BoxedLetterB,
            MobRank.A => SeIconChar.BoxedLetterA,
            MobRank.S => SeIconChar.BoxedLetterS,
            MobRank.SS => SeIconChar.BoxedStar,
            MobRank.FATE => SeIconChar.BoxedLetterF,
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, default),
        };
    }

    public static TextPayload GetRankIcon(MobRank rank)
    {
        return new TextPayload(GetRankIconChar(rank).ToIconString());
    }

    public static TextPayload? GetInstanceIconPayload(int? instance)
    {
        var c = GetInstanceIcon(instance);
        if (!c.HasValue)
        {
            return null;
        }

        return new TextPayload(c.Value.ToIconString());
    }

    public static SeIconChar? GetInstanceIcon(int? instance)
    {
        return instance switch
        {
            1 => SeIconChar.Instance1,
            2 => SeIconChar.Instance2,
            3 => SeIconChar.Instance3,
            4 => SeIconChar.Instance4,
            5 => SeIconChar.Instance5,
            6 => SeIconChar.Instance6,
            7 => SeIconChar.Instance7,
            8 => SeIconChar.Instance8,
            9 => SeIconChar.Instance9,
            _ => default,
        };
    }

    public static string FormatTimeSpan(DateTime time)
    {
        var span = DateTime.UtcNow - time;

        // round TimeSpan
        span = TimeSpan.FromSeconds(Math.Round(span.TotalSeconds));

        var builder = new StringBuilder("(");
        if (span.Days > 0)
        {
            builder.Append(Localization.TimespanDaysAgo.Format(span.Days));
        }
        else if (span.Hours > 0)
        {
            builder.Append(Localization.TimespanHoursAgo.Format(span.Hours));
        }
        else if (span.Minutes > 0)
        {
            builder.Append(Localization.TimespanMinutesAgo.Format(span.Minutes));
        }
        else if (span.Seconds > 10)
        {
            builder.Append(Localization.TimespanSecondsAgo.Format(span.Seconds));
        }
        else
        {
            return string.Empty;
        }

        builder.Append(')');
        return builder.ToString();
    }

    public static SeString CreateMapLink(TerritoryType territoryType, Map map, Vector2 coordinates, int? instance)
    {
        var mapLink = SeString.CreateMapLink(territoryType.RowId, map.RowId, coordinates.X, coordinates.Y);

        var instanceIcon = GetInstanceIconPayload(instance);
        return instanceIcon != default ? mapLink.Append(instanceIcon) : mapLink;
    }

    public static bool IsInDuty(ICondition condition)
    {
        return condition[ConditionFlag.BoundByDuty] || condition[ConditionFlag.BoundByDuty56] || condition[ConditionFlag.BoundByDuty95];
    }
}
