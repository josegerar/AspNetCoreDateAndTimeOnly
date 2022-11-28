using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Converters;

public class TimeOnlyEFCoreConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyEFCoreConverter() : base(
        t => t.ToTimeSpan(),
        dt => TimeOnly.FromTimeSpan(dt))
    {
    }
}