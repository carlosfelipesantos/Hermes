using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Frete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;

        public FreteController(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> Listar()
        {
            var fretes = await _context.Fretes.ToListAsync();
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FreteDTO>> Buscar(int id)
        {
            var frete = await _context.Fretes.FindAsync(id);
            if (frete == null)
                return NotFound();
            return Ok(_mapper.Map<FreteDTO>(frete));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarFrete dto)
        {
            var frete = _mapper.Map<Entities.Frete>(dto);
            _context.Fretes.Add(frete);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<FreteDTO>(frete));



        }
}
