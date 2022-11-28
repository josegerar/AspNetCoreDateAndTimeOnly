using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AspNetCoreDateAndTimeOnly.Comparers;


public class NullableDateOnlyEFCoreComparer : ValueComparer<DateOnly?>
{
    public NullableDateOnlyEFCoreComparer() : base(
        (d1, d2) => d1 == d2 && d1.GetValueOrDefault().DayNumber == d2.GetValueOrDefault().DayNumber,
        d => d.GetHashCode())
    {
    }
}