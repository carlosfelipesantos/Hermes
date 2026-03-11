using Hermes.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Usuario
{
    public class CriarUsuario
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public TipoUsuario Tipo { get; set; }

        [Required]
        public string Telefone { get; set; }

        public string Endereco { get; set; }

        public string FotoPerfil { get; set; }

        [Required]
        public string DDD { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public string Cidade { get; set; }

    }
}
