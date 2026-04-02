using Hermes.Enums;

namespace Hermes.DTOs.Frete
{
    public class CriarFreteAgendadoDTO
    {
        public int TransportadorId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime? DataHoraFimPrevisto { get; set; }
        public TipoCarga TipoCarga { get; set; }
        public bool Urgente { get; set; }
        public string? DDDOrigem { get; set; }
        public string CidadeOrigem { get; set; }
        public string BairroOrigem { get; set; }
        public string EstadoOrigem { get; set; }
        public double LatitudeOrigem { get; set; }
        public double LongitudeOrigem { get; set; }
        public string BairroDestino { get; set; }
        public string CidadeDestino { get; set; }
        public string EstadoDestino { get; set; }
        public double LatitudeDestino { get; set; }
        public double LongitudeDestino { get; set; }
        public string DescricaoCarga { get; set; }
        public decimal Valor { get; set; }
        public bool SitioOrigem { get; set; }
        public string? DescricaoOrigem { get; set; }
        public bool SitioDestino { get; set; }
        public string? DescricaoDestino { get; set; }
        public double? DistanciaExtra { get; set; }
    }
}
