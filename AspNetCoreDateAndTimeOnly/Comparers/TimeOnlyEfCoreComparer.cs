using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AspNetCoreDateAndTimeOnly.Comparers;

internal class TimeOnlyEfCoreComparer : ValueComparer<TimeOnly>
{
    internal TimeOnlyEfCoreComparer() : base(
        (t1, t2) => t1.Ticks == t2.Ticks,
        t => t.GetHashCode())
    {
    }
}