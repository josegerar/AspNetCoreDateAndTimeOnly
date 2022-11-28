using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Converters;

internal class NullableTimeOnlyEFCoreConverter : ValueConverter<TimeOnly?, TimeSpan?>
{
    internal NullableTimeOnlyEFCoreConverter() : base(
        d => d == null
            ? null
            : new TimeSpan?(d.Value.ToTimeSpan()),
        d => d == null
            ? null
            : new TimeOnly?(TimeOnly.FromTimeSpan(d.Value)))
    { }
}
