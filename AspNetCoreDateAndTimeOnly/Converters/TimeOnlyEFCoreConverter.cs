using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Converters;

internal class TimeOnlyEFCoreConverter : ValueConverter<TimeOnly, TimeSpan>
{
    internal TimeOnlyEFCoreConverter() : base(
        t => t.ToTimeSpan(),
        dt => TimeOnly.FromTimeSpan(dt))
    {
    }
}