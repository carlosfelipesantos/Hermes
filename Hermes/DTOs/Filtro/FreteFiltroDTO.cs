using Hermes.Enums;

namespace Hermes.DTOs.Filtro
{
    public class FreteFiltroDTO
    {
        public decimal? ValorMin { get; set; }
        public decimal? ValorMax { get; set; }
        public string? Cidade { get; set; }
        public StatusFrete? Status { get; set; }
     
    }
}
