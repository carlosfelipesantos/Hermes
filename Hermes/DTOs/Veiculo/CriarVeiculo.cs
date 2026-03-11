using Hermes.Enums;

namespace Hermes.DTOs.Veiculo
{
    public class CriarVeiculo
    {
        public TipoVeiculo TipoVeiculo { get; set; }
        public int TransportadorId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Capacidade { get; set; }
    }
}
