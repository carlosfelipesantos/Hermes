using Hermes.DTOs.Disponibilidade;

namespace Hermes.Services.Interfaces
{

    public interface IDisponibilidadeService
    {
        Task CriarDisponibilidade(int transportadorId, DisponibilidadeDTO dto);

        Task<List<TimeSpan>> ListarHorariosDisponiveis(int transportadorId, DateTime data);
    }
}