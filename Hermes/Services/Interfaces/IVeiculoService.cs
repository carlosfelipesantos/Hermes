using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<IEnumerable<Veiculo>> Listar();
        Task<Veiculo> BuscarPorId(int id);
        Task<Veiculo> Criar(Veiculo veiculo);
        Task Atualizar(Veiculo veiculo);
        Task<bool> Deletar(int id);
    }

}
