namespace Hermes.DTOs.Veiculo
{
    public class CriarVeiculo
    {
        public enum TipoVeiculo
        {
            Moto,
            Carro,
            Van,
            Caminhao
        }
        public int TransportadorId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Capacidade { get; set; }
    }
}
