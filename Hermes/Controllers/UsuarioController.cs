using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Usuario;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;
        public UsuarioController(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Listar()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(_mapper.Map<List<UsuarioDTO>>(usuarios));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> Buscar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            return Ok(_mapper.Map<UsuarioDTO>(usuario));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarUsuario dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);
            _context.Usuarios.Add(usuario);

            usuario.Data_Cadastro = DateTime.Now;
            usuario.Ativo = true;

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<UsuarioDTO>(usuario));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, AtualizarUsuario dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            _mapper.Map(dto, usuario);

            await _context.SaveChangesAsync();

            return NoContent();
        }

       [HttpDelete("{id}")]
       public async Task<IActionResult> Deletar(int id)
       {
           var usuario = await _context.Usuarios.FindAsync(id);
           if (usuario == null)
               return NotFound();
           _context.Usuarios.Remove(usuario);
           await _context.SaveChangesAsync();
           return NoContent();
        }
    }
}
