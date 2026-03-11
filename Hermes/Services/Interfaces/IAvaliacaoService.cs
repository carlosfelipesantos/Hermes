using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<Avaliacao> Criar(Avaliacao avaliacao);
    }
}
