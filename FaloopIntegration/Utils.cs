﻿using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Linq;
using System.Text;

namespace Divination.FaloopIntegration;

public static class Utils
{
    public static SeIconChar GetRankIconChar(string rank)
    {
        return rank switch
        {
            "S" => SeIconChar.BoxedLetterS,
            "A" => SeIconChar.BoxedLetterA,
            "B" => SeIconChar.BoxedLetterB,
            "F" => SeIconChar.BoxedLetterF,
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, default),
        };
    }

    public static TextPayload GetRankIcon(string rank)
    {
        return new TextPayload(GetRankIconChar(rank).ToIconString());
    }

    public static TextPayload? GetInstanceIcon(int? instance)
    {
        return instance switch
        {
            1 => new TextPayload(SeIconChar.Instance1.ToIconString()),
            2 => new TextPayload(SeIconChar.Instance2.ToIconString()),
            3 => new TextPayload(SeIconChar.Instance3.ToIconString()),
            4 => new TextPayload(SeIconChar.Instance4.ToIconString()),
            5 => new TextPayload(SeIconChar.Instance5.ToIconString()),
            6 => new TextPayload(SeIconChar.Instance6.ToIconString()),
            7 => new TextPayload(SeIconChar.Instance7.ToIconString()),
            8 => new TextPayload(SeIconChar.Instance8.ToIconString()),
            9 => new TextPayload(SeIconChar.Instance9.ToIconString()),
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

    public static unsafe uint GetStartTownAetheryte()
    {
        var starttown = FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<Town>()?.GetRow(UIState.Instance()->PlayerState.StartTown)?.Name.RawString ?? "null";
        return FaloopIntegration.Instance.Dalamud.DataManager.GetExcelSheet<Aetheryte>()?.FirstOrDefault(a => a.PlaceName.Value!.Name.RawString.Contains(starttown))?.RowId ?? 0;
    }
}
