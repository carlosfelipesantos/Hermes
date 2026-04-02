using System.Text.Json;
using System.Text.Json.Serialization;

public class PortugueseDayOfWeekConverter : JsonConverter<DayOfWeek>
{
    private static readonly Dictionary<string, DayOfWeek> _reverse = new(StringComparer.OrdinalIgnoreCase)
    {
        { "domingo", DayOfWeek.Sunday },
        { "segunda", DayOfWeek.Monday },
        { "segunda-feira", DayOfWeek.Monday },
        { "terca", DayOfWeek.Tuesday },
        { "terça", DayOfWeek.Tuesday },
        { "terca-feira", DayOfWeek.Tuesday },
        { "terça-feira", DayOfWeek.Tuesday },
        { "quarta", DayOfWeek.Wednesday },
        { "quarta-feira", DayOfWeek.Wednesday },
        { "quinta", DayOfWeek.Thursday },
        { "quinta-feira", DayOfWeek.Thursday },
        { "sexta", DayOfWeek.Friday },
        { "sexta-feira", DayOfWeek.Friday },
        { "sabado", DayOfWeek.Saturday },
        { "sábado", DayOfWeek.Saturday }
    };

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

    public override DayOfWeek Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()?.ToLowerInvariant().Trim();
        if (string.IsNullOrEmpty(value))
            throw new JsonException("Valor não pode ser vazio");

        if (_reverse.TryGetValue(value, out var day))
            return day;

        throw new JsonException($"Valor inválido para dia da semana: {value}");
    }

    public override void Write(Utf8JsonWriter writer, DayOfWeek value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_portuguese[value]);
    }
}