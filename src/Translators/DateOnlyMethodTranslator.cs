using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly.Translators;

public class DateOnlyMethodTranslator : IMethodCallTranslator
{
    private readonly Dictionary<MethodInfo, string> _methodInfoDatePartMapping = new()
    {
        { typeof(DateOnly).GetRuntimeMethod(nameof(DateOnly.AddYears), new[] { typeof(int) })!, "year" },
        { typeof(DateOnly).GetRuntimeMethod(nameof(DateOnly.AddMonths), new[] { typeof(int) })!, "month" },
        { typeof(DateOnly).GetRuntimeMethod(nameof(DateOnly.AddDays), new[] { typeof(int) })!, "day" },
    };

    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private readonly IRelationalTypeMappingSource _typeMappingSource;

    public DateOnlyMethodTranslator(IRelationalTypeMappingSource typeMappingSource, ISqlExpressionFactory sqlExpressionFactory)
    {
        _typeMappingSource = typeMappingSource;
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (_methodInfoDatePartMapping.TryGetValue(method, out var datePart)
            && instance != null)
        {
            // DateAdd does not accept number argument outside of int range
            // AddYears/AddMonths take int argument so no need to check for range
            if (datePart != "year"
                && datePart != "month"
                && arguments[0] is SqlConstantExpression sqlConstant
                && sqlConstant.Value is double doubleValue
                && (doubleValue >= int.MaxValue
                    || doubleValue <= int.MinValue))
            {
                return null;
            }

            if (instance is SqlConstantExpression instanceConstant)
            {
                instance = instanceConstant.ApplyTypeMapping(_typeMappingSource.FindMapping(typeof(DateOnly), "date"));
            }

            return _sqlExpressionFactory.Function(
                "DATEADD",
                new[] { _sqlExpressionFactory.Fragment(datePart), _sqlExpressionFactory.Convert(arguments[0], typeof(int)), instance },
                nullable: true,
                argumentsPropagateNullability: new[] { false, true, true },
                instance.Type,
                instance.TypeMapping);
        }

        return null;
    }
}