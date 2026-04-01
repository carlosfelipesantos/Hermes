using System.Text.Json;
using System.Text.Json.Serialization;

public class PortugueseDayOfWeekConverter : JsonConverter<DayOfWeek>
{
    private static readonly Dictionary<DayOfWeek, string> _portuguese = new()
    {
        { DayOfWeek.Sunday, "Domingo" },
        { DayOfWeek.Monday, "Segunda-feira" },
        { DayOfWeek.Tuesday, "Terça-feira" },
        { DayOfWeek.Wednesday, "Quarta-feira" },
        { DayOfWeek.Thursday, "Quinta-feira" },
        { DayOfWeek.Friday, "Sexta-feira" },
        { DayOfWeek.Saturday, "Sábado" }
    };

    private static readonly Dictionary<string, DayOfWeek> _reverse =
        _portuguese.ToDictionary(kv => kv.Value, kv => kv.Key);

    public override DayOfWeek Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        if (_reverse.TryGetValue(value, out var day))
            return day;
        throw new JsonException($"Valor inválido para dia da semana: {value}");
    }

    public override void Write(Utf8JsonWriter writer, DayOfWeek value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_portuguese[value]);
    }
}