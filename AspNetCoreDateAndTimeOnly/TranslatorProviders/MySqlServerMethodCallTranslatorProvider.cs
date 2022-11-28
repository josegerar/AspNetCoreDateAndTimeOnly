﻿using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace AspNetCoreDateAndTimeOnly.TranslatorProviders;

public class MySqlServerMethodCallTranslatorProvider : SqlServerMethodCallTranslatorProvider
{
    public MySqlServerMethodCallTranslatorProvider(RelationalMethodCallTranslatorProviderDependencies dependencies) : base(dependencies)
    {
        var sqlExpressionFactory = dependencies.SqlExpressionFactory;
        var typeMappingSource = dependencies.RelationalTypeMappingSource;

        AddTranslators(new IMethodCallTranslator[]
        {
           // new MySqlServerDateTimeMethodTranslator(sqlExpressionFactory,typeMappingSource)
        });
    }
}