using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Avaliacao;
using Hermes.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;

        public AvaliacaoController(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // LISTAR AVALIAÇÕES DE UM FRETE
        [HttpGet("frete/{freteId}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoDTO>>> ListarPorFrete(int freteId)
        {
            var avaliacoes = await _context.Avaliacoes
                .Where(a => a.FreteId == freteId)
                .ToListAsync();

            return Ok(_mapper.Map<List<AvaliacaoDTO>>(avaliacoes));
        }

        // CRIAR AVALIAÇÃO
        [HttpPost]
        public async Task<ActionResult<AvaliacaoDTO>> Criar(CriarAvaliacao dto)
        {
            var avaliacao = _mapper.Map<Avaliacao>(dto);

            avaliacao.DataAvaliacao = DateTime.Now;

            _context.Avaliacoes.Add(avaliacao);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<AvaliacaoDTO>(avaliacao));
        }
    }
}