using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AspNetCoreDateAndTimeOnly.Converters;

internal class DateOnlyEFCoreConverter : ValueConverter<DateOnly, DateTime>
{
    internal DateOnlyEFCoreConverter() :
        base(d => d.ToDateTime(TimeOnly.MinValue), dt => DateOnly.FromDateTime(dt))
    {
    }
}
