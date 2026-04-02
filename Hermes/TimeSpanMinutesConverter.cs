using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeSpanMinutesConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            return TimeSpan.Zero;

        if (TimeSpan.TryParse(value, out var timeSpan))
            return timeSpan;

        throw new JsonException($"Formato de hora inválido: {value}. Use HH:mm ou HH:mm:ss");
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
       
        writer.WriteStringValue(value.ToString(@"hh\:mm"));
    }
}