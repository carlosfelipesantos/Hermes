using Hermes.Enums;

namespace Hermes.Entities
{
    public class CompatibilidadeCarga
    {
        //Mapear tipo de veiculo para tipo de carga
        private static readonly Dictionary<TipoVeiculo, List<TipoCarga>> Suporte = new()
        {
            { TipoVeiculo.Moto, new List<TipoCarga> {TipoCarga.Pequena}  },
            { TipoVeiculo.Carro, new List<TipoCarga> {TipoCarga.Pequena, TipoCarga.Media} },
             { TipoVeiculo.Carretinha, new List<TipoCarga> { TipoCarga.Pequena, TipoCarga.Media } },
            { TipoVeiculo.Van, new List<TipoCarga> {TipoCarga.Pequena, TipoCarga.Media} },
            {TipoVeiculo.Caminhao, new List<TipoCarga> {TipoCarga.Pequena, TipoCarga.Media, TipoCarga.Grande} }
        };

        public static bool IsCompativel(TipoVeiculo tipoVeiculo, TipoCarga tipoCarga)
        {
            if (Suporte.TryGetValue(tipoVeiculo, out var cargas))
                return cargas.Contains(tipoCarga);
            return false;
        }
    }
}
