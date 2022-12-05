using AspNetCoreDateAndTimeOnly.TranslatorProviders;
using CSharpNetUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetCoreDateAndTimeOnly;

public static class EfCoreExtensions
{
    public static async Task<T?> UseTransaction<T>(this DbContext context, Func<Task<T>> body)
    {
        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }
        using var transaction = context.Database.BeginTransaction();
        try
        {
            var result = await body();

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return result;
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

    public static int? DatePart(this DateTime? date, string datePartArg) =>
        throw new InvalidOperationException($"{nameof(DatePart)} cannot be called client side.");

    public static ModelBuilder AddSqlFunctions(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(() => DatePart(default, default!))
           .HasTranslation(args =>
                    new SqlFunctionExpression("DATEPART",
                        new[]                            {
                                new SqlFragmentExpression((args.ToArray()[1] as SqlConstantExpression)!.Value!.ToString()!),
                                args.ToArray()[0]
                        },
                        true,
                        new[] { false, false },
                        typeof(int?),
                        null
                    )
                );

        modelBuilder.HasDbFunction(() => DateExtensions.ToDateOnly(default))
                .HasTranslation(args => new SqlFunctionExpression(
                    functionName: "CONVERT",
                    arguments: args.Prepend(new SqlFragmentExpression("date")),
                    nullable: true,
                    argumentsPropagateNullability: new[] { false, true },
                    type: typeof(DateOnly),
                    typeMapping: null));

        modelBuilder.HasDbFunction(() => DateExtensions.ToDateOnly(null))
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