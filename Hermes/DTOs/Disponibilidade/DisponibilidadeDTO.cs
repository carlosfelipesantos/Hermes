namespace Hermes.DTOs.Disponibilidade
{
    public class DisponibilidadeDTO
    {
        public DayOfWeek DiaSemana { get; set; }

        public List<TimeSpan> Horas { get; set; }
    }
}