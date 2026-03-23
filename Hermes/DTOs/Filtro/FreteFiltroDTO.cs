using Hermes.Enums;

namespace Hermes.DTOs.Filtro
{
    public class FreteFiltroDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Cidade { get; set; }
        public StatusFrete? Status { get; set; }
        public bool? SomenteDisponiveis { get; set; }
    }
}
