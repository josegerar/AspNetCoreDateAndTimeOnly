using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Storage;

public sealed class TimeOnlyRelationalTypeMapping : RelationalTypeMapping
{
    internal TimeOnlyRelationalTypeMapping(Type type, ValueConverter converter, ValueComparer comparer)
        : this(
            new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(type, converter, comparer),
                storeType: "time",
                dbType: System.Data.DbType.Time))
    { }

    private TimeOnlyRelationalTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        => new TimeOnlyRelationalTypeMapping(parameters);
}