using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Transportador;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;
using Hermes.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportadorController : ControllerBase
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;
    

     public TransportadorController(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("sugestao")]
        public async Task<ActionResult<IEnumerable<TransportadorSugestaoDTO>>> SugerirTransportadores([FromQuery] TipoCarga tipoCarga, [FromQuery] double? latOrigem, [FromQuery] double? lonOrigem, [FromQuery] double raio = 20 )
        {
            var transportadores = await _context.Transportadores.Include(t => t.Veiculos).Where(t => t.Ativo).ToListAsync();

            var result = new List<TransportadorSugestaoDTO>();

            foreach(var t in transportadores)
            {
                //verificar se tem veiculo compativel com tipo da carga
                var veiculoCompativel = t.Veiculos.Any(v => CompatibilidadeCarga.IsCompativel(v.TipoVeiculo, tipoCarga));
                if (!veiculoCompativel) continue;

                //se localizacao for fornecida, calcular distancia
                double distancia =  -1;
                if(latOrigem.HasValue && lonOrigem.HasValue && t.Latitude.HasValue && t.Longitude.HasValue)
                {
                    distancia = CalcularDistancia(latOrigem.Value, lonOrigem.Value, t.Latitude.Value, t.Longitude.Value);
                    if (distancia > raio) continue;
                }

                result.Add(new TransportadorSugestaoDTO
                {
                    Id = t.Id,
                    Nome = t.Nome,
                    Telefone = t.Telefone,
                    DDD = t.DDD,
                    AvaliacaoMedia = t.AvaliacaoMedia,
                    Veiculos = _mapper.Map<List<VeiculoDTO>>(t.Veiculos),
                    DistanciaKm = distancia > 0 ? distancia : (double?)null

                });
            }

            //Ordenar por avaliacao e distancia
            result = result.OrderByDescending(r => r.AvaliacaoMedia).ThenBy(r => r.DistanciaKm ?? double.MaxValue).ToList();
            return Ok(result);

        }

        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            // mesmo método do FreteService 
            const int R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

    } 
}
