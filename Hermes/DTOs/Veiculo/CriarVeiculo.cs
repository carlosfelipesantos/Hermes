using Hermes.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Veiculo
{
    public class CriarVeiculo
    {
        [Required(ErrorMessage = "Tipo de veículo é obrigatório")]
        public TipoVeiculo TipoVeiculo { get; set; }
        public int TransportadorId { get; set; }

        [Required(ErrorMessage = "Marca é obrigatória")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Marca deve ter entre 2 e 50 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Modelo é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Modelo deve ter entre 2 e 50 caracteres")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "Placa é obrigatória")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "Placa inválida")]
        [RegularExpression(@"^[A-Za-z]{3}[0-9]{4}$|^[A-Za-z]{3}[0-9]{1}[A-Za-z]{1}[0-9]{2}$",
            ErrorMessage = "Placa deve estar no formato ABC1234 ou ABC1D23")]
        public string Placa { get; set; }
        public string Capacidade { get; set; }
    }
}
