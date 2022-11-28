using AspNetCoreDateAndTimeOnly.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AspNetCoreDateAndTimeOnly;

public static class JSONExtensions
{
    public static void AddDateAndTimeJsonConverters(this IList<JsonConverter> source)
    {
        source.Add(new DateOnlyJsonConverter());
        source.Add(new DateOnlyNullableJsonConverter());
        source.Add(new TimeOnlyJsonConverter());
        source.Add(new TimeOnlyNullableJsonConverter());
    }
}