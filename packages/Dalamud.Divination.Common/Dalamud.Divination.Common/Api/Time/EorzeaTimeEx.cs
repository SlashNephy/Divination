using System;

namespace Dalamud.Divination.Common.Api.Time;

public static class EorzeaTimeEx
{
    private const double TimeRate = 175.0;
    internal const int MinutesOfHour = 60;
    internal const int HoursOfDay = 24;
    internal const int DaysOfMonth = 32;
    internal const int MonthsOfYear = 12;

    public static DateTime ToEarthTime(this EorzeaTime et)
    {
        var months = MonthsOfYear * (et.Year - 1) + (et.Month - 1);
        var days = DaysOfMonth * months + (et.Day - 1);
        var hours = HoursOfDay * days + et.Hour;
        var minutes = MinutesOfHour * hours + et.Minute;
        var seconds = (long)Math.Round(minutes * TimeRate / MinutesOfHour);

        var utc = DateTimeOffset.FromUnixTimeSeconds(seconds);
        return utc.DateTime;
    }

    public static EorzeaTime ToEorzeaTime(this DateTime lt)
    {
        var utc = (DateTimeOffset)DateTime.SpecifyKind(lt, DateTimeKind.Utc);

        var minutes = (int)Math.Round(utc.ToUnixTimeSeconds() * MinutesOfHour / TimeRate);
        var hours = minutes / MinutesOfHour;
        var days = hours / HoursOfDay;
        var months = days / DaysOfMonth;

        return new EorzeaTime(months / MonthsOfYear + 1,
            months % MonthsOfYear + 1,
            days % DaysOfMonth + 1,
            hours % HoursOfDay,
            minutes % MinutesOfHour);
    }

    public static EorzeaTime Add(this EorzeaTime et,
        int year = default,
        int month = default,
        int day = default,
        int hour = default,
        int minute = default)
    {
        return new EorzeaTime(et.Year + year, et.Month + month, et.Day + day, et.Hour + hour, et.Minute + minute);
    }

    public static EorzeaTime Set(this EorzeaTime et,
        int? year = default,
        int? month = default,
        int? day = default,
        int? hour = default,
        int? minute = default)
    {
        return new EorzeaTime(year ?? et.Year, month ?? et.Month, day ?? et.Day, hour ?? et.Hour, minute ?? et.Minute);
    }
}
