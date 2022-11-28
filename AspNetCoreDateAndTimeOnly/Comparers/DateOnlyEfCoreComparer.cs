using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AspNetCoreDateAndTimeOnly.Comparers;

public class DateOnlyEfCoreComparer : ValueComparer<DateOnly>
{
    public DateOnlyEfCoreComparer() : base(
        (d1, d2) => d1.DayNumber == d2.DayNumber,
        d => d.GetHashCode())
    {
    }
}