namespace RachaStats.Application.Matches;

public static class MatchIdHelper
{
    public static int GetJulianMatchId(DateTime date)
    {
        return date.Year * 1000 + date.DayOfYear;
    }

    public static DateTime GetDateFromJulianMatchId(int matchId)
    {
        var year = matchId / 1000;
        var dayOfYear = matchId % 1000;

        if (year < 1 || year > 9999)
            throw new ArgumentException("matchId inválido.", nameof(matchId));

        var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;

        if (dayOfYear < 1 || dayOfYear > daysInYear)
            throw new ArgumentException("matchId inválido.", nameof(matchId));

        return new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
    }
}
