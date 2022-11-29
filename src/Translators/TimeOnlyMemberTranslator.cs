using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly.Translators;

public class TimeOnlyMemberTranslator : IMemberTranslator
{
    private static readonly Dictionary<string, string> DatePartMappings = new()
    {
        { nameof(TimeOnly.Hour), "hour" },
        { nameof(TimeOnly.Minute), "minute" },
        { nameof(TimeOnly.Second), "second" },
        { nameof(TimeOnly.Millisecond), "millisecond" },
        #if NET7_0_OR_GREATER
            { nameof(TimeOnly.Microsecond), "microsecond" },
            { nameof(TimeOnly.Nanosecond), "nanosecond" },
	    #endif
    };

    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public TimeOnlyMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(SqlExpression? instance, MemberInfo member, Type returnType, 
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (member.DeclaringType == typeof(TimeOnly) && DatePartMappings.TryGetValue(member.Name, out var value))
        {
            return _sqlExpressionFactory.Function(
                "DATEPART", new[] { _sqlExpressionFactory.Fragment(value), instance! },
                nullable: true,
                argumentsPropagateNullability: new[] { false, true },
                returnType);
        }

        return null;
    }
}