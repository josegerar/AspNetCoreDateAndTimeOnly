using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace AspNetCoreDateAndTimeOnly.Storage;

public sealed class DateOnlyRelationalTypeMapping : RelationalTypeMapping
{
    internal DateOnlyRelationalTypeMapping(Type type, ValueConverter converter, ValueComparer comparer)
        : this(
            new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(type, converter, comparer),
                storeType: "date",
                dbType: System.Data.DbType.Date))
    { }

    private DateOnlyRelationalTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        => new DateOnlyRelationalTypeMapping(parameters);
}