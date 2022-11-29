using CSharpNetUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly.Translators;

internal class MySqlServerDateTimeMethodTranslator : IMethodCallTranslator
{
    private Dictionary<MethodInfo, string> _methodInfoDatePartMapping = new()
    {

    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private readonly IRelationalTypeMappingSource _typeMappingSource;

    internal MySqlServerDateTimeMethodTranslator(ISqlExpressionFactory sqlExpressionFactory,
        IRelationalTypeMappingSource typeMappingSource)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
        _typeMappingSource = typeMappingSource;
        var methots = typeof(DateExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(DateExtensions.ToDateOnly))
            .Select(m => new { Method = m, Value = "date" });
        foreach (var item in methots)
        {
            _methodInfoDatePartMapping.Add(item.Method, item.Value);
        }

    }

    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (_methodInfoDatePartMapping.TryGetValue(method, out var datePart))
        {
            switch (method.Name)
            {
                case nameof(DateExtensions.ToDateOnly):
                    return _sqlExpressionFactory.Function(
                        "CONVERT",
                        new[] { _sqlExpressionFactory.Fragment("date") },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false, true },
                        method.ReturnType,
                        _typeMappingSource.FindMapping(typeof(DateOnly)));
            }
        }

        return null;
    }
}
