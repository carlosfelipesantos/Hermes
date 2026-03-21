using Hermes.Entities;

public class Disponibilidade
{
    public int Id { get; set; }

    public int TransportadorId { get; set; }

    public DayOfWeek DiaSemana { get; set; }

    public TimeSpan Hora { get; set; }

    public Transportador Transportador { get; set; }
}