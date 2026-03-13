using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Frete;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        private readonly IFreteService _freteService;
        private readonly IMapper _mapper;

        public FreteController(IFreteService freteService, IMapper mapper)
        {
            _freteService = freteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> Listar()
        {
            var fretes = await _freteService.Listar();
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //ListarPorCliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> ListarPorCliente(int clienteId)
        {
            var fretes = await _freteService.ListarPorCliente(clienteId);

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //FretesDisponiveis
        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> FretesDisponiveis()
        {
            var fretes = await _freteService.ListarDisponiveis();

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //BuscarPorID
        [HttpGet("{id}")]
        public async Task<ActionResult<FreteDTO>> Buscar(int id)
        {
            var frete = await _freteService.BuscarPorId(id);

            if (frete == null)
                return NotFound();

            return Ok(_mapper.Map<FreteDTO>(frete));
        }

        //ListarPorTransportador
        [HttpGet("transportador/{transportadorId}")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> ListarPorTransportador(int transportadorId)
        {
            var fretes = await _freteService.ListarPorTransportador(transportadorId);

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //ListarPorCidade
        [HttpGet("cidade/{cidade}")]
        public async Task<IActionResult> FretesPorCidade(string cidade)
        {
            var fretes = await _freteService.ListarPorCidade(cidade);

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //CriarFrete
        [HttpPost]
        public async Task<IActionResult> Criar(CriarFrete dto)
        {
            var frete = _mapper.Map<Entities.Frete>(dto);

            var freteCriado = await _freteService.Criar(frete);

            return CreatedAtAction(nameof(Buscar), new { id = freteCriado.Id }, _mapper.Map<FreteDTO>(freteCriado));
        }

        //AceitarFrete
        [HttpPost("{id}/aceitar/{transportadorId}")]
        public async Task<IActionResult> AceitarFrete(int id, int transportadorId)
        {
            var sucesso = await _freteService.AceitarFrete(id, transportadorId);

            if (!sucesso)
                return BadRequest();

            return Ok();
        }

        //FinalizarFrete
        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarFrete(int id)
        {
            var sucesso = await _freteService.FinalizarFrete(id);

            if (!sucesso)
                return NotFound();

            return Ok();
        }

        //AtualizarStatusFrete
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, AtualizarStatusFrete dto)
        {
            var sucesso = await _freteService.AtualizarStatus(id, dto.Status);

            if (!sucesso)
                return NotFound();

            return Ok();
        }


        //DeletarFrete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _freteService.Deletar(id);

            if (!sucesso)
                return NotFound();

            return NoContent();
        }

    }
}
