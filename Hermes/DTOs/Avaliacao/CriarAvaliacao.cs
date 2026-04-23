using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Avaliacao
{
    public class CriarAvaliacao
    {
        [Required(ErrorMessage = "Nota é obrigatória")]
        [Range(1, 5, ErrorMessage = "Nota deve ser entre 1 e 5")]
        public double Nota { get; set; }
        public string Comentario { get; set; }

        [Required(ErrorMessage = "Frete é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do frete inválido")]
        public int FreteId { get; set; }

        [Required(ErrorMessage = "Transportador é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do transportador inválido")]
        public int TransportadorId { get; set; }
    }
}
