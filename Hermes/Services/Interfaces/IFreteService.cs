using Hermes.Entities;
using Hermes.Enums;

namespace Hermes.Services.Interfaces
{
    public interface IFreteService
    {
        Task<IEnumerable<Frete>> Listar();

        Task<Frete> BuscarPorId(int id);

        Task<IEnumerable<Frete>> ListarPorCliente(int clienteId);

        Task<IEnumerable<Frete>> ListarPorTransportador(int transportadorId);

        Task<IEnumerable<Frete>> BuscarFretesParaTransportador(int transportadorId);

        Task<IEnumerable<Frete>> ListarPorCidade(string cidade);

        Task<IEnumerable<Frete>> ListarDisponiveis();

        Task<Frete> Criar(Frete frete);

        Task<bool> AceitarFrete(int freteId, int transportadorId);

        Task<bool> FinalizarFrete(int id);

        Task<bool> AtualizarStatus(int id, StatusFrete status);

        Task<bool> Deletar(int id);
    }
}