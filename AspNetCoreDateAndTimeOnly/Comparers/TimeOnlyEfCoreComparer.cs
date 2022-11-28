using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AspNetCoreDateAndTimeOnly.Comparers;

public class TimeOnlyEfCoreComparer : ValueComparer<TimeOnly>
{
    public TimeOnlyEfCoreComparer() : base(
        (t1, t2) => t1.Ticks == t2.Ticks,
        t => t.GetHashCode())
    {
    }
}