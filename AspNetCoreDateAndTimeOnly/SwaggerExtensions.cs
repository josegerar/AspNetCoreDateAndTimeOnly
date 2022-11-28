using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace AspNetCoreDateAndTimeOnly;

public static class SwaggerExtensions
{
    public static void UseDateOnlyTimeOnlyStringConverters(this SwaggerGenOptions options)
    {
        options.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date"
        });
        options.MapType<TimeOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
        });
    }
}