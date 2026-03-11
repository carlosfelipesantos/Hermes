using Hermes.Enums;

namespace Hermes.DTOs.Veiculo
{
    public class VeiculoDTO
    {
        public int Id { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Capacidade { get; set; }
        public bool Disponivel { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
