using Hermes.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Frete
{
    public class CriarFrete
    {
        [Required(ErrorMessage = "Tipo de carga é obrigatório")]
        public TipoCarga TipoCarga { get; set; }
        public bool Urgente { get; set; }

        [StringLength(5, ErrorMessage = "DDD deve ter no máximo 5 caracteres")]
        public string? DDDOrigem { get; set; }
        public string CidadeOrigem { get; set; }

        [Required(ErrorMessage = "Bairro de origem é obrigatório")]
        [StringLength(100, ErrorMessage = "Bairro deve ter no máximo 100 caracteres")]
        public string BairroOrigem { get; set; }

        [Required(ErrorMessage = "Estado de origem é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve ter 2 caracteres")]
        public string EstadoOrigem { get; set; }
        public double? LatitudeOrigem { get; set; }
        public double? LongitudeOrigem { get; set; }

        public double? LatitudeDestino { get; set; }
        public double? LongitudeDestino { get; set; }
        public string? BairroDestino { get; set; }

        [Required(ErrorMessage = "Cidade de destino é obrigatória")]
        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string CidadeDestino { get; set; }

        [Required(ErrorMessage = "Estado de destino é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve ter 2 caracteres")]
        public string EstadoDestino { get; set; }

        [Required(ErrorMessage = "Descrição da carga é obrigatória")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Descrição deve ter entre 3 e 500 caracteres")]
        public string DescricaoCarga { get; set; }


        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor deve ser entre R$ 0,01 e R$ 999.999,99")]
        public decimal Valor { get; set; }
        
        public bool SitioOrigem { get; set; }
        public string? DescricaoOrigem { get; set; }
        public bool SitioDestino { get; set; }
        public string? DescricaoDestino { get; set; }
        public double? DistanciaExtra { get; set; }

       
    }
}
