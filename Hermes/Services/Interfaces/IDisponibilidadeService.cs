using Hermes.DTOs.Disponibilidade;

namespace Hermes.Services.Interfaces
{

    public interface IDisponibilidadeService
    {
        Task CriarDisponibilidade(int transportadorId, DisponibilidadeDTO dto);

        Task<List<TimeSpan>> ListarHorariosDisponiveis(int transportadorId, DateTime data);

        Task<List<DisponibilidadeDTO>> ListarDisponibilidadesPorTransportador(int transportadorId);
        Task<bool> AtualizarDisponibilidade(int disponibilidadeId, AtualizarDisponibilidadeDTO dto, int transportadorId);
        Task<bool> DeletarDisponibilidade(int disponibilidadeId, int transportadorId);
    }
}