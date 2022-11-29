using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetCoreDateAndTimeOnly.Comparers;

internal class NullableDateOnlyEFCoreComparer : ValueComparer<DateOnly?>
{
    internal NullableDateOnlyEFCoreComparer() : base(
        (d1, d2) => d1 == d2 && d1.GetValueOrDefault().DayNumber == d2.GetValueOrDefault().DayNumber,
        d => d.GetHashCode())
    {
    }
}