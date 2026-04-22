using Hermes.DTOs.Transportador;
using Hermes.Enums;

namespace Hermes.Services.Interfaces
{
    public interface ITransportadorService
    {
        Task<List<TransportadorSugestaoDTO>> SugerirTransportadores(
            TipoCarga tipoCarga,
            double? latOrigem,
            double? lonOrigem,
            double raio = 20);
    }
}