using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Notificacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacaoController : ControllerBase
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;

        public NotificacaoController(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // LISTAR NOTIFICAÇÕES DO USUÁRIO
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<NotificacaoDTO>>> ListarPorUsuario(int usuarioId)
        {
            var notificacoes = await _context.Notificacoes
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.DataCriacao)
                .ToListAsync();

            return Ok(_mapper.Map<List<NotificacaoDTO>>(notificacoes));
        }

        // MARCAR COMO LIDA
        [HttpPut("{id}/lida")]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            var notificacao = await _context.Notificacoes.FindAsync(id);

            if (notificacao == null)
                return NotFound();

            notificacao.Lida = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}