using Hermes.Entities;

namespace Hermes.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> Listar();
        Task<Usuario> BuscarPorId(int id); 
        Task<Usuario> Criar(Usuario usuario);
        Task<bool> Deletar(int id);
    }
}
