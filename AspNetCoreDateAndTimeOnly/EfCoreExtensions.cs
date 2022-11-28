using AspNetCoreDateAndTimeOnly.TranslatorProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDateAndTimeOnly;

public static class EfCoreExtensions
{
    public static async Task UseTransaction(this DbContext context, Func<Task> body)
    {
        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }
        using var transaction = context.Database.BeginTransaction();
        try
        {
            await body();
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public static async Task UseTransaction(this DbContext context, Action body)
    {
        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }
        using var transaction = context.Database.BeginTransaction();
        try
        {
            body();
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public static ModelBuilder AddSqlFunctions(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(() => DateAndTimeOnlyExtensions.ToDateOnly(default))
                .HasTranslation(args => new SqlFunctionExpression(
                    functionName: "CONVERT",
                    arguments: args.Prepend(new SqlFragmentExpression("date")),
                    nullable: true,
                    argumentsPropagateNullability: new[] { false, true },
                    type: typeof(DateOnly),
                    typeMapping: null));

        modelBuilder.HasDbFunction(() => DateAndTimeOnlyExtensions.ToDateOnly(null))
                .HasTranslation(args => new SqlFunctionExpression(
                    functionName: "CONVERT",
                    arguments: args.Prepend(new SqlFragmentExpression("date")),
                    nullable: true,
                    argumentsPropagateNullability: new[] { false, true },
                    type: typeof(DateOnly),
                    typeMapping: null));

        return modelBuilder;
    }

    public static DbContextOptionsBuilder AddSuportDateAndTimeSqlServer(this DbContextOptionsBuilder options)
    {
        options.ReplaceService<IRelationalTypeMappingSource, MySqlServerTypeMappingSource>();
        options.ReplaceService<IMethodCallTranslatorProvider, MySqlServerMethodCallTranslatorProvider>();
        options.ReplaceService<IMemberTranslatorProvider, MySqlServerMemberTranslatorProvider>();

        return options;
    }
}