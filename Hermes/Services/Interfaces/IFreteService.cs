using Hermes.DTOs.Filtro;
using Hermes.DTOs.Paginacao;
using Hermes.Entities;
using Hermes.Enums;

namespace Hermes.Services.Interfaces
{
    public interface IFreteService
    {
        //para home
        Task<List<Frete>> ListarConcluidosRecentes(int quantidade);

        //filtrados
        Task<(List<Frete> data, int total)> ListarDisponiveisFiltrado(
            FreteFiltroDTO filtro, PaginacaoParams paginacao);

        // paginação geral
        Task<(List<Frete> data, int total)> ListarPaginado(int page, int pageSize);

        // paginação específica
        Task<(List<Frete> data, int total)> ListarDisponiveisPaginado(int transportadorId, int page, int pageSize);
        Task<(List<Frete> data, int total)> ListarPorCidadePaginado(string cidade, int page, int pageSize);

        //  sem paginação
        Task<IEnumerable<Frete>> Listar();
        Task<Frete> BuscarPorId(int id);
        Task<IEnumerable<Frete>> ListarPorCliente(int clienteId);
        Task<IEnumerable<Frete>> ListarPorTransportador(int transportadorId);
        Task<IEnumerable<Frete>> BuscarFretesParaTransportador(int transportadorId);
        Task<IEnumerable<Frete>> ListarPorCidade(string cidade);
        Task<IEnumerable<Frete>> ListarDisponiveis();
        Task<Frete> Criar(Frete frete);
        Task<bool> AceitarFrete(int freteId, int transportadorId);
        Task<bool> FinalizarFrete(int id, int transportadorId);
        Task<bool> AtualizarStatus(int id, StatusFrete status);
        Task<bool> Deletar(int id);
    }
}