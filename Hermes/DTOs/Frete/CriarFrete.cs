using Hermes.Enums;

namespace Hermes.DTOs.Frete
{
    public class CriarFrete
    {
        public TipoCarga TipoCarga { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public string DescricaoCarga { get; set; }
        public double Valor { get; set; }
    }
}
