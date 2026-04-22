using AutoMapper;
using Hermes.DTOs.Notificacao;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hermes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        public NotificacaoController(INotificacaoService notificacaoService, IMapper mapper)
        {
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        // Notificacoes do usuario logado
        [HttpGet("minhas")]
        public async Task<ActionResult<IEnumerable<NotificacaoDTO>>> MinhasNotificacoes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var notificacoes = await _notificacaoService.ListarNotificacoesPorUsuario(userId);

            return Ok(_mapper.Map<List<NotificacaoDTO>>(notificacoes));
        }

        // Marcar como lida (Somente o dono)
        [HttpPut("{id}/lida")]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var sucesso = await _notificacaoService.MarcarComoLida(id, userId);

            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }
}