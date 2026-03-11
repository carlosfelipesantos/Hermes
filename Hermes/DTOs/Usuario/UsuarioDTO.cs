using Hermes.Enums;

namespace Hermes.DTOs.Usuario
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoUsuario Tipo { get; set; }
    }
}
