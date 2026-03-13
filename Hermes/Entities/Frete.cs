using Hermes.Enums;

namespace Hermes.Entities
{
    public class Frete
    {
     

        public int Id { get; set; }
        public TipoCarga TipoCarga { get; set; }

        public double LatitudeOrigem { get; set; }
        public double LongitudeOrigem { get; set; }

        public double LatitudeDestino { get; set; }
        public double LongitudeDestino { get; set; }

        public string CidadeOrigem { get; set; }
        public string BairroOrigem { get; set; }
 
        public string EstadoOrigem { get; set; }
        public string BairroDestino { get; set; }
        public string CidadeDestino { get; set; }
        public string EstadoDestino { get; set; }
        public string DescricaoCarga { get; set; }
        public double Valor { get; set; }
        public StatusFrete Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataConclusao { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int? TransportadorId { get; set; }
        public Transportador? Transportador { get; set; }

        public Avaliacao? Avaliacao { get; set; }

    }
}
