using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;
using Hermes.Services.Interfaces;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoDTO>>> Listar()
        {
            var veiculos = await _veiculoService.Listar();
            return Ok(_mapper.Map<List<VeiculoDTO>>(veiculos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VeiculoDTO>> Buscar(int id)
        {
            var veiculo = await _veiculoService.BuscarPorId(id);

            if (veiculo == null)
                return NotFound();

            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarVeiculo dto)
        {
            var veiculo = _mapper.Map<Veiculo>(dto);

            await _veiculoService.Criar(veiculo);

            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarVeiculo dto)
        {
            var veiculo = await _veiculoService.BuscarPorId(id);

            if (veiculo == null)
                return NotFound();

            _mapper.Map(dto, veiculo);

            await _veiculoService.Atualizar(veiculo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _veiculoService.Deletar(id);

            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }
}
