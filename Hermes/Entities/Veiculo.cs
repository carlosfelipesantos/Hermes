using System;
namespace Hermes.Entities
{
    public class Veiculo
    {
        public int Id { get; set; }
        public enum TipoVeiculo { 
            Moto,
            Carro,
            Van,
            Caminhao
        }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public string Capacidade { get; set; }
        public bool Disponivel { get; set; }
        public DateTime DataCadastro { get; set; }

        public int TransportadorId { get; set; }
        public Transportador Transportador { get; set; }
    }
}
