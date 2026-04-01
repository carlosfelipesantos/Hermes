namespace Hermes.Entities
{
    public class DisponibilidadeBase
    {
        public int Id { get; set; }
        public int TransportadorId { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public Transportador Transportador { get; set; }
    }
}
