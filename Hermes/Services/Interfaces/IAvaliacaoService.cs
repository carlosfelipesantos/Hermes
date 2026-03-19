using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<Avaliacao> Criar(Avaliacao avaliacao);
        Task<IEnumerable<Avaliacao>> ListarPorTransportador(int transportadorId);

        Task<double> CalcularMediaTransportador(int transportadorId);


    }
}
