namespace Extensions.Core;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime date) => DateOnly.FromDateTime(date);
}