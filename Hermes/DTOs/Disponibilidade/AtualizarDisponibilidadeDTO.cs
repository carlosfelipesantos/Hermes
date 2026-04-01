using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Disponibilidade
{
    public class AtualizarDisponibilidadeDTO
    {
        [Required]
        public DayOfWeek DiaSemana { get; set; }
       
        [Required]
        public TimeSpan Hora { get; set; }
    }
}
