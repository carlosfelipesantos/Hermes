using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IFreteService
    {
        Task<IEnumerable<Frete>> Listar();
        Task<Frete> BuscarPorId(int id);
        Task<Frete> Criar(Frete frete);
        Task<bool> AceitarFrete(int freteId, int transportadoraId);
        Task<bool> FinalizarFrete(int id);


    }
}
