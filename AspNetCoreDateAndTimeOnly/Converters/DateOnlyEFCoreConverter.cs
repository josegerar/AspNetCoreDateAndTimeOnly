using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Converters;

public class DateOnlyEFCoreConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyEFCoreConverter() :
        base(d => d.ToDateTime(TimeOnly.MinValue), dt => DateOnly.FromDateTime(dt))
    {
    }
}
