using CSharpNetUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly.Translators;

internal class MySqlServerDateExtensionMethodsTranslator : IMethodCallTranslator
{
    private Dictionary<MethodInfo, string> _methodInfoDateConvertMapping = new()
    {
        {
            typeof(DateExtensions).GetMethod(
                nameof(DateExtensions.ToDateOnly),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateTime) },
                null
             )!,
            "date"
        },
        {
            typeof(DateExtensions).GetMethod(
                nameof(DateExtensions.ToDateOnly),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateTime?) },
                null
             )!,
            "date"
        },
        {
            typeof(DateExtensions).GetMethod(
                nameof(DateExtensions.ToDateTime),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateOnly) },
                null
             )!,
            "datetime"
        },
        {
            typeof(DateExtensions).GetMethod(
                nameof(DateExtensions.ToDateTime),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateOnly?) },
                null
             )!,
            "datetime"
        }
    };
    private List<MethodInfo> _methodInfoDatePartMapping = new()
    {
        typeof(EfCoreExtensions).GetMethod(
                nameof(EfCoreExtensions.DatePart),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateTime?), typeof(string) },
                null
             )!,
        typeof(EfCoreExtensions).GetMethod(
                nameof(EfCoreExtensions.DatePart),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateTime), typeof(string) },
                null
             )!,
        typeof(EfCoreExtensions).GetMethod(
                nameof(EfCoreExtensions.DatePart),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateOnly?), typeof(string) },
                null
             )!,
        typeof(EfCoreExtensions).GetMethod(
                nameof(EfCoreExtensions.DatePart),
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DateOnly), typeof(string) },
                null
             )!
    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private readonly IRelationalTypeMappingSource _typeMappingSource;

    internal MySqlServerDateExtensionMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory,
        IRelationalTypeMappingSource typeMappingSource)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
        _typeMappingSource = typeMappingSource;
    }

    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (_methodInfoDateConvertMapping.TryGetValue(method, out var datePart))
        {
            return _sqlExpressionFactory.Function(
                        "CONVERT",
                        new[] { _sqlExpressionFactory.Fragment(datePart), arguments[0] },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false, true },
                        method.ReturnType,
                        _typeMappingSource.FindMapping(method.ReturnType));
        }

        foreach (var item in _methodInfoDatePartMapping)
        {
            if (method == item)
            {
                var datepart = ((arguments[1] as SqlConstantExpression)!.Value as string)!;

                if (datepart == null) return null;

                return _sqlExpressionFactory.Function(
                        "DATEPART",
                        new[] {
                            _sqlExpressionFactory.Fragment(datepart),
                            arguments[0]
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false, true },
                        method.ReturnType);
            }
        }

        return null;
    }
}
