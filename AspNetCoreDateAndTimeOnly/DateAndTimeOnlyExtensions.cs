using System;

namespace AspNetCoreDateAndTimeOnly;
public static class DateAndTimeOnlyExtensions
{
    public static DateOnly ToDateOnly(this DateTime source) => DateOnly.FromDateTime(source);

    public static DateOnly? ToDateOnly(this DateTime? source)
    {
        if (!source.HasValue)
        {
            return null;
        }
        try
        {
            return source.Value.ToDateOnly();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static DateTime? ToDateTime(this DateOnly? source)
    {
        if (!source.HasValue)
        {
            return null;
        }
        try
        {
            return source.Value.ToDateTime();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static DateTime ToDateTime(this DateOnly source) => source.ToDateTime(TimeOnly.MinValue);

}