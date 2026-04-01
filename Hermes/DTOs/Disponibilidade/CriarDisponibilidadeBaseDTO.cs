namespace Hermes.DTOs.Disponibilidade
{
    public class CriarDisponibilidadeBaseDTO
    {
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
    }
}
