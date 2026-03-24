using Hermes.Entities;
using Hermes.Enums;

namespace Hermes.DTOs.Frete
{
    public class FreteDTO
    {
        public TipoCarga TipoCarga { get; set; }

        public int Id { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }

        public string DescricaoCarga { get; set; }
        public decimal Valor { get; set; }

        public StatusFrete Status { get; set; }

        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataConclusao { get; set; }

        public string? TelefoneTransportador { get; set; }
        public string? MensagemWhatsapp { get; set; }

        public bool SitioOrigem { get; set; }
        public string? DescricaoOrigem { get; set; }
        public bool SitioDestino { get; set; }
        public string? DescricaoDestino { get; set; }
        public double? DistanciaExtra { get; set; }

        public int ClienteId { get; set; }
        public int? TransportadorId { get; set; }

        public string NomeCliente { get; set; }
        public string? NomeTransportador { get; set; }
    }
}
