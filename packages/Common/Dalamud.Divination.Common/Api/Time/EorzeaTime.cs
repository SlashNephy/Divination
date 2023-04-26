using System;

namespace Dalamud.Divination.Common.Api.Time;

public readonly struct EorzeaTime
{
    public readonly int Year;
    public readonly int Month;
    public readonly int Day;
    public readonly int Hour;
    public readonly int Minute;

    public EorzeaTime(int year, int month, int day, int hour, int minute)
    {
        if (month > EorzeaTimeEx.MonthsOfYear)
        {
            month -= EorzeaTimeEx.MonthsOfYear;
            year++;
        }

        Year = year;

        if (day > EorzeaTimeEx.DaysOfMonth)
        {
            day -= EorzeaTimeEx.DaysOfMonth;
            month++;
        }

        Month = month;

        if (hour > EorzeaTimeEx.HoursOfDay)
        {
            hour -= EorzeaTimeEx.HoursOfDay;
            day++;
        }

        Day = day;

        if (minute > EorzeaTimeEx.MinutesOfHour)
        {
            minute -= EorzeaTimeEx.MinutesOfHour;
            hour++;
        }

        Minute = minute;
        Hour = hour;
    }

    public static EorzeaTime Now => DateTime.UtcNow.ToEorzeaTime();
}
