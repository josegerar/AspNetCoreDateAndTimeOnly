using AspNetCoreDateAndTimeOnly.Converters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AspNetCoreDateAndTimeOnly;

public static class AspNetCoreExtensions
{
    /// <summary>
    /// Da soporte a la api para los tipos de datos DateOnly y TimeOnly, para net 6
    /// </summary>
    /// <param name="options">El MvcOptions del IMvcBuilder</param>
    /// <returns>MvcOptions</returns>
    public static MvcOptions UseDateOnlyTimeOnlyStringConverters(this MvcOptions options)
    {
        TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
        TypeDescriptor.AddAttributes(typeof(TimeOnly), new TypeConverterAttribute(typeof(TimeOnlyTypeConverter)));
        return options;
    }
}