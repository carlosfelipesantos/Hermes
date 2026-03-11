using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<IEnumerable<Veiculo>> Listar();
        Task<Veiculo> Criar(Veiculo veiculo);
        Task<bool> Deletar(int id);
    }
}
