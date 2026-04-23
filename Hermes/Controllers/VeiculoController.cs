using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Hermes.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculoController : ControllerBase
    {
        private readonly IVeiculoService _veiculoService;
        private readonly IMapper _mapper;

        public VeiculoController(IVeiculoService veiculoService, IMapper mapper)
        {
            _veiculoService = veiculoService;
            _mapper = mapper;
        }

        [Authorize] // Qualquer usuário logado
        [HttpGet("transportador/{transportadorId}")]
        public async Task<IActionResult> VeiculosPorTransportador(int transportadorId)
        {
            var veiculos = await _veiculoService.ListarPorTransportador(transportadorId);

            return Ok(_mapper.Map<List<VeiculoDTO>>(veiculos));
        }

        // Veiculos do usuario logado
        [Authorize(Roles = "Transportador")]
        [HttpGet("meus")]
        public async Task<ActionResult<IEnumerable<VeiculoDTO>>> Meus()
        {
            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var veiculos = await _veiculoService.ListarPorTransportador(transportadorId);

            return Ok(_mapper.Map<List<VeiculoDTO>>(veiculos));
        }

        // Buscar (somente o dono)
        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoDTO>> Buscar(int id)
        {
            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var veiculo = await _veiculoService.BuscarPorId(id);

            if (veiculo == null || veiculo.TransportadorId != transportadorId)
                return NotFound();

            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

        // Criar (vinculado ao usuario logado)
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarVeiculo dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var veiculo = _mapper.Map<Veiculo>(dto);
            veiculo.TransportadorId = transportadorId;

            await _veiculoService.Criar(veiculo);

            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

        //  Atualizar (somente o dono)
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarVeiculo dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var veiculo = await _veiculoService.BuscarPorId(id);

            if (veiculo == null || veiculo.TransportadorId != transportadorId)
                return NotFound();

            _mapper.Map(dto, veiculo);

            await _veiculoService.Atualizar(veiculo);

            return NoContent();
        }

        //  Deletar (somente o dono)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var transportadorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );

            var veiculo = await _veiculoService.BuscarPorId(id);

            if (veiculo == null || veiculo.TransportadorId != transportadorId)
                return NotFound();

            await _veiculoService.Deletar(id);

            return NoContent();
        }
    }
}