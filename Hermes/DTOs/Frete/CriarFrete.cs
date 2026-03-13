using Hermes.Enums;

namespace Hermes.DTOs.Frete
{
    public class CriarFrete
    {
        public TipoCarga TipoCarga { get; set; }
        public string CidadeOrigem { get; set; }
        public string BairroOrigem { get; set; }
        public string EstadoOrigem { get; set; }
        public double LatitudeOrigem { get; set; }
        public double LongitudeOrigem { get; set; }

        public double LatitudeDestino { get; set; }
        public double LongitudeDestino { get; set; }
        public string BairroDestino { get; set; }
        public string CidadeDestino { get; set; }
        public string EstadoDestino { get; set; }
        public string DescricaoCarga { get; set; }
        public double Valor { get; set; }
        public int ClienteId { get; set; }
    }
}
