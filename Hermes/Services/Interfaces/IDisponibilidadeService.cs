using Hermes.DTOs.Disponibilidade;

namespace Hermes.Services.Interfaces
{

    public interface IDisponibilidadeService
    {
        Task<List<DisponibilidadeBaseDTO>> ListarJanelasPorTransportador(int transportadorId);
        Task CriarJanela(int transportadorId, CriarDisponibilidadeBaseDTO dto);
        Task AtualizarJanela(int janelaId, CriarDisponibilidadeBaseDTO dto, int transportadorId);
        Task<bool> DeletarJanela(int janelaId, int transportadorId);

        Task<List<IntervaloLivreDTO>> ListarIntervalosLivres(int transportadorId, DateTime data, TimeSpan? buffer = null);
    }
}