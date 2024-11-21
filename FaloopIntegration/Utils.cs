using System;
using System.Numerics;
using System.Text;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin.Services;
using Lumina.Excel.Sheets;

namespace Divination.FaloopIntegration;

public static class Utils
{
    public static string GetRankIcon(MobRank rank)
    {
        return rank switch
        {
            MobRank.B => SeIconChar.BoxedLetterB.ToIconString(),
            MobRank.A => SeIconChar.BoxedLetterA.ToIconString(),
            MobRank.S => SeIconChar.BoxedLetterS.ToIconString(),
            MobRank.SS => SeIconChar.BoxedStar.ToIconString(),
            MobRank.FATE => SeIconChar.BoxedLetterF.ToIconString(),
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, "invalid rank"),
        };
    }

    public static string GetInstanceIcon(int instance)
    {
        return instance switch
        {
            0 => string.Empty,
            1 => SeIconChar.Instance1.ToIconString(),
            2 => SeIconChar.Instance2.ToIconString(),
            3 => SeIconChar.Instance3.ToIconString(),
            4 => SeIconChar.Instance4.ToIconString(),
            5 => SeIconChar.Instance5.ToIconString(),
            6 => SeIconChar.Instance6.ToIconString(),
            7 => SeIconChar.Instance7.ToIconString(),
            8 => SeIconChar.Instance8.ToIconString(),
            9 => SeIconChar.Instance9.ToIconString(),
            _ => throw new ArgumentOutOfRangeException(nameof(instance), instance, "invalid instance number"),
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

    public static SeString CreateMapLink(TerritoryType territoryType, Map map, Vector2 coordinates, int instance)
    {
        var mapLink = SeString.CreateMapLink(territoryType.RowId, map.RowId, coordinates.X, coordinates.Y);

        // append instance if instance available
        var instanceIcon = new TextPayload(GetInstanceIcon(instance));
        return mapLink.Append(instanceIcon);
    }

    public static bool IsInDuty(ICondition condition)
    {
        return condition[ConditionFlag.BoundByDuty] || condition[ConditionFlag.BoundByDuty56] || condition[ConditionFlag.BoundByDuty95];
    }
}
