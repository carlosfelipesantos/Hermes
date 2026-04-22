using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Notificacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hermes.Controllers
{
    [Authorize]
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

        // Notificacoes do usuario logado
        [HttpGet("minhas")]
        public async Task<ActionResult<IEnumerable<NotificacaoDTO>>> MinhasNotificacoes()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var notificacoes = await _context.Notificacoes
                .Where(n => n.UsuarioId == userId)
                .OrderByDescending(n => n.DataCriacao)
                .ToListAsync();

            return Ok(_mapper.Map<List<NotificacaoDTO>>(notificacoes));
        }

        // Marcar como lida (Somente o dono)
        [HttpPut("{id}/lida")]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var notificacao = await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Id == id && n.UsuarioId == userId);

            if (notificacao == null)
                return NotFound();

            notificacao.Lida = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}