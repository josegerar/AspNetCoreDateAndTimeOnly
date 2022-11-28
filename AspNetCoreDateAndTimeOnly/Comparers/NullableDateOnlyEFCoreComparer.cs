using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AspNetCoreDateAndTimeOnly.Comparers;


internal class NullableDateOnlyEFCoreComparer : ValueComparer<DateOnly?>
{
    internal NullableDateOnlyEFCoreComparer() : base(
        (d1, d2) => d1 == d2 && d1.GetValueOrDefault().DayNumber == d2.GetValueOrDefault().DayNumber,
        d => d.GetHashCode())
    {
    }
}