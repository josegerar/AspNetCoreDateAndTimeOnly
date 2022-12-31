using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly.Translators;

internal class MySqlServerObjectToStringTranslator : IMethodCallTranslator
{
    private const int DefaultLength = 100;

    private static readonly Dictionary<Type, string> _typeMapping
            = new()
            {
                { typeof(DateOnly), $"varchar({DefaultLength})" },
                { typeof(TimeOnly), $"varchar({DefaultLength})" },
            };

    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    internal MySqlServerObjectToStringTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method,
        IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        //SqlServerObjectToStringTranslator

        if (instance == null || method.Name != nameof(ToString) || arguments.Count != 0)
        {
            return null;
        }

        return _typeMapping.TryGetValue(instance.Type, out var storeType)
               ? _sqlExpressionFactory.Function(
                   "CONVERT",
                   new[] { _sqlExpressionFactory.Fragment(storeType), instance },
                   nullable: true,
                   argumentsPropagateNullability: new[] { false, true },
                   typeof(string))
               : null;
    }
}