using AspNetCoreDateAndTimeOnly.Comparers;
using AspNetCoreDateAndTimeOnly.Converters;
using AspNetCoreDateAndTimeOnly.Storage;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreDateAndTimeOnly.TranslatorProviders;

internal class MySqlServerTypeMappingSource : SqlServerTypeMappingSource
{
    private readonly DateOnlyRelationalTypeMapping _date = new(typeof(DateOnly),
        new DateOnlyEFCoreConverter(), new DateOnlyEfCoreComparer());

    private readonly DateOnlyRelationalTypeMapping _dateNullable = new(typeof(DateOnly?),
        new NullableDateOnlyEFCoreConverter(), new NullableDateOnlyEFCoreComparer());

    private readonly TimeOnlyRelationalTypeMapping _time = new(typeof(TimeOnly),
        new TimeOnlyEFCoreConverter(), new TimeOnlyEfCoreComparer());

    private readonly TimeOnlyRelationalTypeMapping _timeNullable = new(typeof(TimeOnly?),
        new NullableTimeOnlyEFCoreConverter(), new NullableTimeOnlyEFCoreComparer());

    private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;
    internal MySqlServerTypeMappingSource(TypeMappingSourceDependencies dependencies,
        RelationalTypeMappingSourceDependencies relationalDependencies)
        : base(dependencies, relationalDependencies)
    {
        _clrTypeMappings
            = new Dictionary<Type, RelationalTypeMapping>
            {
                { typeof(DateOnly), _date },
                { typeof(DateOnly?), _dateNullable },
                { typeof(TimeOnly), _time },
                { typeof(TimeOnly?), _timeNullable },
            };
    }

    protected override RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
        => base.FindMapping(mappingInfo) ?? FindRawMapping(mappingInfo)?.Clone(mappingInfo);

    private RelationalTypeMapping? FindRawMapping(RelationalTypeMappingInfo mappingInfo)
    {
        var clrType = mappingInfo.ClrType;
        var storeTypeName = mappingInfo.StoreTypeName;

        if (clrType != null)
        {
            if (_clrTypeMappings.TryGetValue(clrType, out var mapping))
            {
                return mapping;
            }
        }

        return null;
    }
}

