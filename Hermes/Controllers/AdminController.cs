using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Frete;
using Hermes.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;

        public AdminController(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> ListarUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Usuarios.AsNoTracking();

            var total = await query.CountAsync();

            var usuarios = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var usuariosDTO = _mapper.Map<List<UsuarioDTO>>(usuarios);

            return Ok(new {

                Data = usuariosDTO,
                Total = total,
                Page = page,
                PageSize = pageSize

            });
        }

        [HttpGet("fretes")]
        public async Task<IActionResult> ListarFretes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Fretes.AsNoTracking().Include(f => f.Cliente).Include(f => f.Transportador);

            var total = await query.CountAsync();

            var fretes = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

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
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Ativo = ativo;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    } 
}

