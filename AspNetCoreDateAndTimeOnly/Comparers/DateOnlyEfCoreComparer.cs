using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AspNetCoreDateAndTimeOnly.Comparers;

internal class DateOnlyEfCoreComparer : ValueComparer<DateOnly>
{
    internal DateOnlyEfCoreComparer() : base(
        (d1, d2) => d1.DayNumber == d2.DayNumber,
        d => d.GetHashCode())
    {
    }
}