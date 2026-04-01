using Hermes.DTOs.Veiculo;

namespace Hermes.DTOs.Transportador
{
    public class TransportadorSugestaoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string DDD { get; set; }
        public double? AvaliacaoMedia { get; set; }
        public List<VeiculoDTO> Veiculos { get; set; }
        public double? DistanciaKm { get; set; }
    }
}
