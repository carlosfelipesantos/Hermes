using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Transportador;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Hermes.Utils;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class TransportadorService : ITransportadorService
    {
        private readonly HermesBD _context;
        private readonly IMapper _mapper;

        public TransportadorService(HermesBD context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TransportadorSugestaoDTO>> SugerirTransportadores(
            TipoCarga tipoCarga,
            double? latOrigem,
            double? lonOrigem,
            double raio = 20)
        {
            var transportadores = await _context.Transportadores
                .Include(t => t.Veiculos)
                .Where(t => t.Ativo)
                .ToListAsync();

            if (!transportadores.Any())
                throw new NotFoundException("Nenhum transportador ativo encontrado");

            var result = new List<TransportadorSugestaoDTO>();

            foreach (var t in transportadores)
            {
                // Verificar se tem veículo compatível com o tipo da carga
                var veiculoCompativel = t.Veiculos.Any(v =>
                    CompatibilidadeCarga.IsCompativel(v.TipoVeiculo, tipoCarga));

                if (!veiculoCompativel) continue;

                // Se localização for fornecida, calcular distância
                double distancia = -1;
                if (latOrigem.HasValue && lonOrigem.HasValue &&
                    t.Latitude.HasValue && t.Longitude.HasValue)
                {
                    distancia = CalcularDistancia(
                        latOrigem.Value, lonOrigem.Value,
                        t.Latitude.Value, t.Longitude.Value);

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

            if (!result.Any())
                throw new NotFoundException("Nenhum transportador encontrado com os critérios informados");

            // Ordenar por avaliação e distância
            result = result
                .OrderByDescending(r => r.AvaliacaoMedia)
                .ThenBy(r => r.DistanciaKm ?? double.MaxValue)
                .ToList();

            return result;
        }

        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            const int R = 6371; // Raio da Terra em km
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