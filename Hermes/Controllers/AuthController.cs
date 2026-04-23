using Hermes.DTOs.Auth;
using Hermes.Entities;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        private string GerarToken(Usuario user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key não configurada"))
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Tipo.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["ExpireMinutes"] ?? "60")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _usuarioService.Autenticar(login.Email, login.Senha);
                var token = GerarToken(user);

                return Ok(new
                {
                    token,
                    usuario = new
                    {
                        user.Id,
                        user.Email,
                        user.Tipo
                    }
                });
            }
            catch (NotFoundException)
            {
                return Unauthorized("Credenciais inválidas");
            }
            catch (BusinessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}