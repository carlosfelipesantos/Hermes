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
        public double Valor { get; set; }

        public StatusFrete Status { get; set; }

        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataConclusao { get; set; }

        public int ClienteId { get; set; }
        public int? TransportadorId { get; set; }

        public string NomeCliente { get; set; }
        public string? NomeTransportador { get; set; }
    }
}
