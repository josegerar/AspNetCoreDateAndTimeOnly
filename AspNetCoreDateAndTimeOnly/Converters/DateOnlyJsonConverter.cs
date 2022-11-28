using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreDateAndTimeOnly.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString(), AspNetCoreDateAndTimeOnlyConstants.Format.DateOnly, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(AspNetCoreDateAndTimeOnlyConstants.Format.DateOnly, CultureInfo.InvariantCulture));
    }
}

public class DateOnlyNullableJsonConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null)
        {
            return null;
        }
        try
        {
            return DateOnly.ParseExact(value, AspNetCoreDateAndTimeOnlyConstants.Format.DateOnly, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(AspNetCoreDateAndTimeOnlyConstants.Format.DateOnly, CultureInfo.InvariantCulture));
    }
}
