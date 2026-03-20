using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
