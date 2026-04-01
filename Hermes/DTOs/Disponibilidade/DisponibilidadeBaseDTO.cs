namespace Hermes.DTOs.Disponibilidade
{
    public class DisponibilidadeBaseDTO
    {
        public int Id { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
    }
}
