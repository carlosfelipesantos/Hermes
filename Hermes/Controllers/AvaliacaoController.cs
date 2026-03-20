using AutoMapper;
using Hermes.DTOs.Avaliacao;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly IMapper _mapper;

        public AvaliacaoController(IAvaliacaoService avaliacaoService, IMapper mapper)
        {
            _avaliacaoService = avaliacaoService;
            _mapper = mapper;
        }

        // 🔓 PODE SER PÚBLICO (ver avaliações do transportador)
        [HttpGet("transportador/{transportadorId}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoDTO>>> ListarPorTransportador(int transportadorId)
        {
            var avaliacoes = await _avaliacaoService.ListarPorTransportador(transportadorId);
            return Ok(_mapper.Map<List<AvaliacaoDTO>>(avaliacoes));
        }

        // 🔓 MÉDIA DO TRANSPORTADOR
        [HttpGet("transportador/{transportadorId}/media")]
        public async Task<ActionResult<double>> MediaTransportador(int transportadorId)
        {
            var media = await _avaliacaoService.CalcularMediaTransportador(transportadorId);
            return Ok(media);
        }

        // 🔒 CRIAR AVALIAÇÃO (SÓ CLIENTE LOGADO)
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<ActionResult<AvaliacaoDTO>> Criar(CriarAvaliacao dto)
        {
            try
            {
                var clienteId = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier).Value
                );

                var avaliacao = _mapper.Map<Avaliacao>(dto);

                // 🔥 FORÇA o cliente correto
                avaliacao.ClienteId = clienteId;

                var avaliacaoCriada = await _avaliacaoService.Criar(avaliacao);

                return Ok(_mapper.Map<AvaliacaoDTO>(avaliacaoCriada));
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}