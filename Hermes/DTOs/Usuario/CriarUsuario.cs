using Hermes.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Usuario
{
    public class CriarUsuario
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Senha deve ter entre 4 e 50 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public TipoUsuario Tipo { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Telefone inválido")]
        [Phone(ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; }

        public string Endereco { get; set; }

        public string FotoPerfil { get; set; }

        [Required(ErrorMessage = "DDD é obrigatório")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "DDD inválido")]
        public string DDD { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve ter 2 caracteres")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Cidade deve ter entre 2 e 100 caracteres")]

        public string Cidade { get; set; }

    }
}
