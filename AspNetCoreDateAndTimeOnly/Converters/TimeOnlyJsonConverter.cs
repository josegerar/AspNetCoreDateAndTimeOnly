using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreDateAndTimeOnly.Converters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString(), AspNetCoreDateAndTimeOnlyConstants.Format.TimeOnly, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(AspNetCoreDateAndTimeOnlyConstants.Format.TimeOnly, CultureInfo.InvariantCulture));
    }
}

public class TimeOnlyNullableJsonConverter : JsonConverter<TimeOnly?>
{
    public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null)
        {
            return null;
        }
        try
        {
            return TimeOnly.ParseExact(value, AspNetCoreDateAndTimeOnlyConstants.Format.TimeOnly, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(AspNetCoreDateAndTimeOnlyConstants.Format.TimeOnly, CultureInfo.InvariantCulture));
    }
}