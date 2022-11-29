using AspNetCoreDateAndTimeOnly.Translators;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreDateAndTimeOnly.TranslatorProviders;

public class MySqlServerMemberTranslatorProvider : SqlServerMemberTranslatorProvider
{
    public MySqlServerMemberTranslatorProvider(RelationalMemberTranslatorProviderDependencies dependencies,
        IRelationalTypeMappingSource typeMappingSource) : base(dependencies, typeMappingSource)
    {
        ISqlExpressionFactory expressionFactory = dependencies.SqlExpressionFactory;

        AddTranslators(
            new IMemberTranslator[]
            {
                    new DateOnlyMemberTranslator(expressionFactory),
                    new TimeOnlyMemberTranslator(expressionFactory)
            }
        );

    }
}


