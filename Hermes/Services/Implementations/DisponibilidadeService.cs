using Hermes.Data;
using Hermes.DTOs.Disponibilidade;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class DisponibilidadeService : IDisponibilidadeService
    {
        private readonly HermesBD _context;

        public DisponibilidadeService(HermesBD context)
        {
            _context = context;
        }

        public async Task CriarDisponibilidade(int transportadorId, DisponibilidadeDTO dto)
        {
            var disponibilidades = dto.Horas.Select(h => new Disponibilidade
            {
                TransportadorId = transportadorId,
                DiaSemana = dto.DiaSemana,
                Hora = h
            });

            _context.Disponibilidades.AddRange(disponibilidades);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TimeSpan>> ListarHorariosDisponiveis(int transportadorId, DateTime data)
        {
            var diaSemana = data.DayOfWeek;

            // horários que o transportador definiu
            var disponibilidades = await _context.Disponibilidades
                .Where(d => d.TransportadorId == transportadorId && d.DiaSemana == diaSemana)
                .Select(d => d.Hora)
                .ToListAsync();

            // horários já ocupados (fretes)
            var ocupados = await _context.Fretes
                .Where(f => f.TransportadorId == transportadorId && f.DataAgendada == data.Date)
                .Select(f => f.HoraAgendada.Value)
                .ToListAsync();

            // remove ocupados
            var disponiveis = disponibilidades
                .Where(h => !ocupados.Contains(h))
                .ToList();

            return disponiveis;
        }


        public async Task<List<DisponibilidadeDTO>> ListarDisponibilidadesPorTransportador(int transportadorId)
        {
            var disponibilidades = await _context.Disponibilidades
                .Where(d => d.TransportadorId == transportadorId)
                .OrderBy(d => d.DiaSemana)
                .ThenBy(d => d.Hora)
                .ToListAsync();

            // Agrupar por dia da semana
            var grupos = disponibilidades.GroupBy(d => d.DiaSemana)
                .Select(g => new DisponibilidadeDTO
                {
                    DiaSemana = g.Key,
                    Horas = g.Select(d => d.Hora).OrderBy(h => h).ToList()
                })
                .OrderBy(dto => dto.DiaSemana)
                .ToList();

            return grupos;
        }

        public async Task<bool> AtualizarDisponibilidade(int disponibilidadeId, AtualizarDisponibilidadeDTO dto, int transportadorId)
        {
            var disponibilidade = await _context.Disponibilidades
                .FirstOrDefaultAsync(d => d.Id == disponibilidadeId && d.TransportadorId == transportadorId);
            if (disponibilidade == null)
                return false;

            disponibilidade.DiaSemana = dto.DiaSemana;
            disponibilidade.Hora = dto.Hora;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarDisponibilidade(int disponibilidadeId, int transportadorId)
        {
            var disponibilidade = await _context.Disponibilidades
                .FirstOrDefaultAsync(d => d.Id == disponibilidadeId && d.TransportadorId == transportadorId);
            if (disponibilidade == null)
                return false;

            _context.Disponibilidades.Remove(disponibilidade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}