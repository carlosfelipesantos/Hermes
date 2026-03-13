using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Avaliacao;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

     
        [HttpGet("frete/{freteId}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoDTO>>> ListarPorFrete(int transportadorId)
        {
            var avaliacoes = await _avaliacaoService.ListarPorTransportador(transportadorId);
              
            return Ok(_mapper.Map<List<AvaliacaoDTO>>(avaliacoes));
        }

    
        [HttpPost]
        public async Task<ActionResult<AvaliacaoDTO>> Criar(CriarAvaliacao dto)
        {
            var avaliacao = _mapper.Map<Avaliacao>(dto);

            var avaliacaoCriada = await _avaliacaoService.Criar(avaliacao);

            return Ok(_mapper.Map<AvaliacaoDTO>(avaliacao));
        }
    }
}