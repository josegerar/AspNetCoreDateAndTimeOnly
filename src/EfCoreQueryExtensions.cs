using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace AspNetCoreDateAndTimeOnly;

public static class EfCoreQueryExtensions
{
    public async static Task InsertarParametrosPaginacionEnRespuesta<T>(this HttpContext? context,
    IQueryable<T> queryable, int cantidadRegistrosAMostrar)
    {
        if (context == null) { return; }

        //contamos registros de la entidad
        double conteo = await queryable.CountAsync();
        //calculo el total de paginas 1000 registros/10 paginas
        double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosAMostrar);
        context.Response.Headers.Add("conteo", conteo.ToString());
        context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
    }

    public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionEFCore paginacion, bool activarPaginacion = true)
    {
        if (activarPaginacion)
        {
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistrosPorPagina)
                .Take(paginacion.CantidadRegistrosPorPagina);
        }
        else
        {
            return queryable;
        }
    }

    public static DbTransaction GetDbTransaction(this IDbContextTransaction source)
    {
        return ((IInfrastructure<DbTransaction>)source).Instance;
    }

    public static async Task<T?> ExecuteScalar<T>(this DbContext context,
    string sql, params object[] parameters)
    {
        using var cmd = context.Database.GetDbConnection().CreateCommand();
        if (cmd.Connection?.State != ConnectionState.Open)
        {
            await cmd.Connection!.OpenAsync();
        }
        cmd.CommandText = sql;
        if (parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        return (T?)await cmd.ExecuteScalarAsync();
    }

    public static async IAsyncEnumerable<T> FromSqlQuery<T>(this DbContext context, string query, params object[] parameters) where T : new()
    {
        var t = typeof(T);
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        List<PropertyMapp> entityFields = (from PropertyInfo aProp in typeof(T).GetProperties(flags)
                                           select new PropertyMapp
                                           {
                                               Name = aProp.Name,
                                               Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                                           }).ToList();
        List<PropertyMapp> dbDataReaderFields = new();
        List<PropertyMapp> commonFields = null!;

        using var command = context.Database.GetDbConnection().CreateCommand();
        if (command.Connection?.State != ConnectionState.Open)
        {
            await command.Connection!.OpenAsync();
        }
        var currentTransaction = context.Database.CurrentTransaction;
        if (currentTransaction != null)
        {
            command.Transaction = currentTransaction.GetDbTransaction();
        }
        command.CommandText = query;
        if (parameters.Any())
        {
            command.Parameters.AddRange(parameters);
        }
        using var result = await command.ExecuteReaderAsync();
        while (await result.ReadAsync())
        {
            if (t.IsPrimitive || t == typeof(string) || t == typeof(DateTime) || t == typeof(Guid) || t == typeof(decimal))
            {
                var val = await result.IsDBNullAsync(0) ? null : result[0];
                yield return (T)val!;
            }
            else
            {
                if (commonFields == null)
                {
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        dbDataReaderFields.Add(new PropertyMapp { Name = result.GetName(i), Type = result.GetFieldType(i) });
                    }
                    commonFields = entityFields.Where(x => dbDataReaderFields.Any(d => d.IsSame(x))).Select(x => x).ToList();
                }

                var entity = new T();
                foreach (var aField in commonFields)
                {
                    PropertyInfo? propertyInfos = entity.GetType().GetProperty(aField.Name);
                    var value = result[aField.Name] == DBNull.Value ? null : result[aField.Name]; //if field is nullable
                    propertyInfos?.SetValue(entity, value, null);
                }
                yield return entity;
            }
        }
    }

    public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>(this IQueryable<TEntity> source,
        bool condition, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        where TEntity : class
    {
        return condition ? source.Include(navigationPropertyPath) : source;
    }
}