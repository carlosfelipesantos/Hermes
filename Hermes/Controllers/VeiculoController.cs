using Hermes.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


using Hermes.DTOs.Veiculo;
using Hermes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController] //dizendo que essa classe é controlador de api
    [Route("api/[controller]")] //definindo a rota para acessar os métodos desse controlador, o [controller] é um placeholder que será substituído pelo nome do controlador, nesse caso "veiculo"
    public class VeiculoController : ControllerBase 
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;

        public VeiculoController(HermesBD context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoDTO>>> Listar()
        {
            var veiculos = await _context.Veiculos.ToListAsync();
            return Ok(_mapper.Map<List<VeiculoDTO>>(veiculos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoDTO>> Buscar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound();
            
            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarVeiculo veiculoDto)
        {
            var veiculo = _mapper.Map<Veiculo>(veiculoDto);

            var transportadorExiste = await _context.Transportadores
        .AnyAsync(t => t.Id == veiculo.TransportadorId);

            if (!transportadorExiste)
                return BadRequest(new { message = "TransportadorId inválido ou inexistente." });


            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();
            
            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, CriarVeiculo dto)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            _mapper.Map(dto, veiculo);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return NotFound();

            _context.Veiculos.Remove(veiculo);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
