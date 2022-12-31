using AspNetCoreDateAndTimeOnly.Comparers;
using AspNetCoreDateAndTimeOnly.Converters;
using AspNetCoreDateAndTimeOnly.Storage;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreDateAndTimeOnly.TranslatorProviders;

public class MySqlServerTypeMappingSource : SqlServerTypeMappingSource
{
    private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings = new()
    {
        {
            typeof(DateOnly),
            new DateOnlyRelationalTypeMapping(
                typeof(DateOnly),
                new DateOnlyEFCoreConverter(),
                new DateOnlyEfCoreComparer()
            )
        },
                {
            typeof(DateOnly?),
            new DateOnlyRelationalTypeMapping(
                typeof(DateOnly?),
                new NullableDateOnlyEFCoreConverter(),
                new NullableDateOnlyEFCoreComparer()
            )
        },
                {
            typeof(TimeOnly),
            new TimeOnlyRelationalTypeMapping(
                typeof(TimeOnly),
                new TimeOnlyEFCoreConverter(),
                new TimeOnlyEfCoreComparer()
            )
        },
                {
            typeof(TimeOnly?),
            new TimeOnlyRelationalTypeMapping(
                typeof(TimeOnly?),
                new NullableTimeOnlyEFCoreConverter(),
                new NullableTimeOnlyEFCoreComparer()
            )
        },
    };

    public MySqlServerTypeMappingSource(TypeMappingSourceDependencies dependencies,
        RelationalTypeMappingSourceDependencies relationalDependencies)
        : base(dependencies, relationalDependencies) { }

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

