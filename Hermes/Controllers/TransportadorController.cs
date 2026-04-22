using Hermes.DTOs.Transportador;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportadorController : ControllerBase
    {
        private readonly ITransportadorService _transportadorService;

        public TransportadorController(ITransportadorService transportadorService)
        {
            _transportadorService = transportadorService;
        }

        [HttpGet("sugestao")]
        public async Task<ActionResult<IEnumerable<TransportadorSugestaoDTO>>> SugerirTransportadores(
            [FromQuery] TipoCarga tipoCarga,
            [FromQuery] double? latOrigem,
            [FromQuery] double? lonOrigem,
            [FromQuery] double raio = 20)
        {
            var resultado = await _transportadorService.SugerirTransportadores(
                tipoCarga, latOrigem, lonOrigem, raio);

            return Ok(resultado);
        }
    }
}