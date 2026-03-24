using Hermes.Enums;

namespace Hermes.DTOs.Frete
{
    public class FretePublicoDTO
    {
        public TipoCarga TipoCarga { get; set; }

        public int Id { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }

        public StatusFrete Status { get; set; }

        public DateTime DataSolicitacao { get; set; }
    
        public string? NomeTransportador { get; set; }
    }
}