using Hermes.Enums;

namespace Hermes.DTOs.Filtro
{
    public class FreteFiltroDTO
    {
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? DDD { get; set; }
        public decimal? ValorMin { get; set; }
        public decimal? ValorMax { get; set; }
        public bool? Urgente { get; set; }
        public StatusFrete? Status { get; set; }
     
    }
}
