using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Frete;
using Hermes.Enums;
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

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> ListarPorCliente(int clienteId)
        {
            var fretes = await _context.Fretes
                .Where(f => f.ClienteId == clienteId)
                .ToListAsync();

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> FretesDisponiveis()
        {
            var fretes = await _context.Fretes
                .Where(f => f.TransportadorId == null)
                .ToListAsync();

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

        [HttpGet("transportador/{transportadorId}")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> ListarPorTransportador(int transportadorId)
        {
            var fretes = await _context.Fretes
                .Where(f => f.TransportadorId == transportadorId)
                .ToListAsync();

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarFrete dto)
        {
            var frete = _mapper.Map<Entities.Frete>(dto);

            frete.Status = StatusFrete.Pendente;
            frete.DataSolicitacao = DateTime.Now;

            _context.Fretes.Add(frete);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Buscar), new { id = frete.Id }, _mapper.Map<FreteDTO>(frete));
        }

        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarFrete(int id)
        {
            var frete = await _context.Fretes.FindAsync(id);

            if (frete == null)
                return NotFound();

            frete.Status = StatusFrete.Concluido;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<FreteDTO>(frete));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, AtualizarStatusFrete dto)
        { 
            var frete = await _context.Fretes.FindAsync(id); // Busca o frete pelo ID
            if (frete == null)
                return NotFound(); // Retorna 404 se o frete não for encontrado
            frete.Status = dto.Status; // Atualiza o status do frete
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            return Ok(_mapper.Map<FreteDTO>(frete)); // Retorna o frete atualizado

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var frete = await _context.Fretes.FindAsync(id);
            if (frete == null)
                return NotFound();
            _context.Fretes.Remove(frete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
