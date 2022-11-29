using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AspNetCoreDateAndTimeOnly.Converters;

internal class TimeOnlyEFCoreConverter : ValueConverter<TimeOnly, TimeSpan>
{
    internal TimeOnlyEFCoreConverter() : base(
        t => t.ToTimeSpan(),
        dt => TimeOnly.FromTimeSpan(dt))
    {
    }
}