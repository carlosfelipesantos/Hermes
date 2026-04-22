using AutoMapper;
using Hermes.DTOs.Frete;
using Hermes.DTOs.Usuario;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> ListarUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Limitar pageSize para evitar sobrecarga
            pageSize = pageSize > 100 ? 100 : pageSize;

            var (usuarios, total) = await _adminService.ListarUsuarios(page, pageSize);
            var usuariosDTO = _mapper.Map<List<UsuarioDTO>>(usuarios);

            return Ok(new
            {
                Data = usuariosDTO,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpGet("fretes")]
        public async Task<IActionResult> ListarFretes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Limitar pageSize para evitar sobrecarga
            pageSize = pageSize > 100 ? 100 : pageSize;

            var (fretes, total) = await _adminService.ListarFretes(page, pageSize);
            var fretesDTO = _mapper.Map<List<FreteDTO>>(fretes);

            return Ok(new
            {
                Data = fretesDTO,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpPut("usuarios/{id}/status")]
        public async Task<IActionResult> AtualizarStatusUsuario(int id, [FromBody] bool ativo)
        {
            var sucesso = await _adminService.AtualizarStatusUsuario(id, ativo);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }
}