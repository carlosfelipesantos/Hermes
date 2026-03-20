using AutoMapper;
using Hermes.DTOs.Frete;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hermes.Controllers
{
    [Authorize]
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

        // Listar todos (admin ou uso geral)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> Listar()
        {
            var fretes = await _freteService.Listar();
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // 🔥 LISTAR FRETES DO CLIENTE LOGADO
        [Authorize(Roles = "Cliente")]
        [HttpGet("meus")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> MeusFretes()
        {
            var clienteId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var fretes = await _freteService.ListarPorCliente(clienteId);

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // 🔥 FRETES DISPONÍVEIS (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> FretesDisponiveis()
        {
            var fretes = await _freteService.ListarDisponiveis();
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // Buscar por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FreteDTO>> Buscar(int id)
        {
            var frete = await _freteService.BuscarPorId(id);

            if (frete == null)
                return NotFound();

            return Ok(_mapper.Map<FreteDTO>(frete));
        }

        // 🔥 LISTAR FRETES DO TRANSPORTADOR LOGADO
        [Authorize(Roles = "Transportador")]
        [HttpGet("meus-transportes")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> MeusTransportes()
        {
            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var fretes = await _freteService.ListarPorTransportador(transportadorId);

            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // Listar por cidade (público autenticado)
        [HttpGet("cidade/{cidade}")]
        public async Task<IActionResult> FretesPorCidade(string cidade)
        {
            var fretes = await _freteService.ListarPorCidade(cidade);
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // 🔥 CRIAR FRETE (Cliente)
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Criar(CriarFrete dto)
        {
            var clienteId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var frete = _mapper.Map<Entities.Frete>(dto);
            frete.ClienteId = clienteId;

            var freteCriado = await _freteService.Criar(frete);

            return CreatedAtAction(
                nameof(Buscar),
                new { id = freteCriado.Id },
                _mapper.Map<FreteDTO>(freteCriado)
            );
        }

        // 🔥 ACEITAR FRETE (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/aceitar")]
        public async Task<IActionResult> AceitarFrete(int id)
        {
            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var sucesso = await _freteService.AceitarFrete(id, transportadorId);

            if (!sucesso)
                return BadRequest("Frete indisponível ou já aceito");

            return Ok();
        }

        // 🔥 FINALIZAR FRETE (Transportador dono do frete)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarFrete(int id)
        {
            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var sucesso = await _freteService.FinalizarFrete(id, transportadorId);

            if (!sucesso)
                return NotFound();

            return Ok();
        }

        // Atualizar status (caso precise)
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, AtualizarStatusFrete dto)
        {
            var sucesso = await _freteService.AtualizarStatus(id, dto.Status);

            if (!sucesso)
                return NotFound();

            return Ok();
        }

        // Deletar
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