using Hermes.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


using Hermes.DTOs.Veiculo;
using Hermes.Entities;

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

        [HttpPost]
        public async Task<IActionResult> Criar(CriarVeiculo veiculoDto)
        {
            var veiculo = _mapper.Map<Veiculo>(veiculoDto);

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();
            
            return Ok(_mapper.Map<VeiculoDTO>(veiculo));
        }

    }
}
