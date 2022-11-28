using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
namespace AspNetCoreDateAndTimeOnly.Comparers;

public class NullableTimeOnlyEFCoreComparer : ValueComparer<TimeOnly?>
{
    public NullableTimeOnlyEFCoreComparer() : base(
        (d1, d2) => d1 == d2 && d1.GetValueOrDefault().Ticks == d2.GetValueOrDefault().Ticks,
        d => d.GetHashCode())
    {
    }
}