using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Converters;

internal class DateOnlyEFCoreConverter : ValueConverter<DateOnly, DateTime>
{
    internal DateOnlyEFCoreConverter() :
        base(d => d.ToDateTime(TimeOnly.MinValue), dt => DateOnly.FromDateTime(dt))
    {
    }
}
