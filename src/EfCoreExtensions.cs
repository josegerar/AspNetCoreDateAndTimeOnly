using AspNetCoreDateAndTimeOnly.TranslatorProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    public static int? DatePart(this DateTime? date, string datePartArg1) =>
        throw new InvalidOperationException($"{nameof(DatePart)} cannot be called client side.");

    public static int DatePart(this DateTime date, string datePartArg2) =>
        throw new InvalidOperationException($"{nameof(DatePart)} cannot be called client side.");

    public static int? DatePart(this DateOnly? date, string datePartArg3) =>
        throw new InvalidOperationException($"{nameof(DatePart)} cannot be called client side.");

    public static int DatePart(this DateOnly date, string datePartArg4) =>
        throw new InvalidOperationException($"{nameof(DatePart)} cannot be called client side.");

    public static DbContextOptionsBuilder AddSuportDateAndTimeSqlServer(this DbContextOptionsBuilder options)
    {
        options.ReplaceService<IRelationalTypeMappingSource, MySqlServerTypeMappingSource>();
        options.ReplaceService<IMethodCallTranslatorProvider, MySqlServerMethodCallTranslatorProvider>();
        options.ReplaceService<IMemberTranslatorProvider, MySqlServerMemberTranslatorProvider>();

        return options;
    }
}