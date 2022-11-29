using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly.Translators;

internal class DateOnlyMemberTranslator : IMemberTranslator
{
    private static readonly Dictionary<string, string> DatePartMapping = new()
        {
            { nameof(DateOnly.Year), "year" },
            { nameof(DateOnly.Month), "month" },
            { nameof(DateOnly.DayOfYear), "dayofyear" },
            { nameof(DateOnly.Day), "day" },
            { nameof(DateOnly.DayOfWeek), "weekday" },
        };

    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    internal DateOnlyMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(SqlExpression? instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        Type? declaringType = member.DeclaringType;

        if ((declaringType == typeof(DateOnly) || declaringType == typeof(DateOnly?))
            && DatePartMapping.TryGetValue(member.Name, out string? datePart))
        {
            return _sqlExpressionFactory.Function(
                "DATEPART",
                new[] { _sqlExpressionFactory.Fragment(datePart), instance! },
                nullable: true,
                argumentsPropagateNullability: new[] { false, true },
                returnType);
        }

        return null;
    }
}
