using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IAdminService
    {
        Task<(List<Usuario> data, int total)> ListarUsuarios(int page, int pageSize);
        Task<(List<Frete> data, int total)> ListarFretes(int page, int pageSize);
        Task<bool> AtualizarStatusUsuario(int id, bool ativo);
    }
}
