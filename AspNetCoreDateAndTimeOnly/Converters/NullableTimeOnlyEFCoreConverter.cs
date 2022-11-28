using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Converters;

public class NullableTimeOnlyEFCoreConverter : ValueConverter<TimeOnly?, TimeSpan?>
{
    public NullableTimeOnlyEFCoreConverter() : base(
        d => d == null
            ? null
            : new TimeSpan?(d.Value.ToTimeSpan()),
        d => d == null
            ? null
            : new TimeOnly?(TimeOnly.FromTimeSpan(d.Value)))
    { }
}
