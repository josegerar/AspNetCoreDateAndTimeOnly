using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
namespace AspNetCoreDateAndTimeOnly.Comparers;

internal class NullableTimeOnlyEFCoreComparer : ValueComparer<TimeOnly?>
{
    internal NullableTimeOnlyEFCoreComparer() : base(
        (d1, d2) => d1 == d2 && d1.GetValueOrDefault().Ticks == d2.GetValueOrDefault().Ticks,
        d => d.GetHashCode())
    {
    }
}