using Hermes.Data;
using Hermes.DTOs.Disponibilidade;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class DisponibilidadeService : IDisponibilidadeService
    {
        private readonly HermesBD _context;
        private readonly TimeSpan _bufferPadrao = TimeSpan.FromMinutes(20);

        public DisponibilidadeService(HermesBD context)
        {
            _context = context;
        }

        public async Task<List<DisponibilidadeBaseDTO>> ListarJanelasPorTransportador(int transportadorId)
        {
            var janelas = await _context.DisponibilidadesBase
                .Where(d => d.TransportadorId == transportadorId)
                .Select(d => new DisponibilidadeBaseDTO
                {
                    Id = d.Id,
                    DiaSemana = d.DiaSemana,
                    HoraInicio = d.HoraInicio,
                    HoraFim = d.HoraFim
                })
                .ToListAsync();

            if (!janelas.Any())
                throw new NotFoundException($"Nenhuma janela de disponibilidade encontrada para o transportador {transportadorId}");

            return janelas;
        }

        public async Task CriarJanela(int transportadorId, CriarDisponibilidadeBaseDTO dto)
        {
            // Validação básica
            if (dto.HoraInicio >= dto.HoraFim)
                throw new BusinessException("Horário de início deve ser menor que o horário de fim.");

            // Validar sobreposição com janelas existentes
            var sobreposicao = await _context.DisponibilidadesBase
                .AnyAsync(d => d.TransportadorId == transportadorId &&
                               d.DiaSemana == dto.DiaSemana &&
                               d.HoraInicio < dto.HoraFim &&
                               d.HoraFim > dto.HoraInicio);

            if (sobreposicao)
                throw new ConflictException("Já existe uma janela de disponibilidade neste horário.");

            var janela = new DisponibilidadeBase
            {
                TransportadorId = transportadorId,
                DiaSemana = dto.DiaSemana,
                HoraInicio = dto.HoraInicio,
                HoraFim = dto.HoraFim
            };
            _context.DisponibilidadesBase.Add(janela);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarJanela(int janelaId, CriarDisponibilidadeBaseDTO dto, int transportadorId)
        {
            var janela = await _context.DisponibilidadesBase
                .FirstOrDefaultAsync(d => d.Id == janelaId && d.TransportadorId == transportadorId);
            if (janela == null)
                throw new NotFoundException($"Janela {janelaId} não encontrada para o transportador {transportadorId}");

            if (dto.HoraInicio >= dto.HoraFim)
                throw new BusinessException("Horário de início deve ser menor que o horário de fim.");

            // Validar sobreposição com outras janelas (exceto a própria)
            var sobreposicao = await _context.DisponibilidadesBase
                .AnyAsync(d => d.TransportadorId == transportadorId &&
                               d.Id != janelaId &&
                               d.DiaSemana == dto.DiaSemana &&
                               d.HoraInicio < dto.HoraFim &&
                               d.HoraFim > dto.HoraInicio);

            if (sobreposicao)
                throw new ConflictException("Já existe uma janela de disponibilidade neste horário.");

            janela.DiaSemana = dto.DiaSemana;
            janela.HoraInicio = dto.HoraInicio;
            janela.HoraFim = dto.HoraFim;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletarJanela(int janelaId, int transportadorId)
        {
            var janela = await _context.DisponibilidadesBase
                .FirstOrDefaultAsync(d => d.Id == janelaId && d.TransportadorId == transportadorId);
            if (janela == null)
                throw new NotFoundException($"Janela {janelaId} não encontrada para o transportador {transportadorId}");

            _context.DisponibilidadesBase.Remove(janela);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<IntervaloLivreDTO>> ListarIntervalosLivres(int transportadorId, DateTime data, TimeSpan? buffer = null)
        {
            buffer ??= _bufferPadrao;
            var diaSemana = data.DayOfWeek;

            // Obter janelas de trabalho para o dia da semana
            var janelas = await _context.DisponibilidadesBase
                .Where(d => d.TransportadorId == transportadorId && d.DiaSemana == diaSemana)
                .Select(d => new { d.HoraInicio, d.HoraFim })
                .ToListAsync();

            if (!janelas.Any())
                return new List<IntervaloLivreDTO>();

            // Obter fretes agendados para esse transportador que começam no dia (ou cruzam)
            var inicioDia = data.Date;
            var fimDia = data.Date.AddDays(1);
            var fretes = await _context.Fretes
                .Where(f => f.TransportadorId == transportadorId &&
                            f.DataHoraInicio >= inicioDia && f.DataHoraInicio < fimDia &&
                            f.Status != StatusFrete.Cancelado)
                .Select(f => new { f.DataHoraInicio, f.DataHoraFimPrevisto })
                .ToListAsync();

            // Para cada janela, calcular intervalos livres
            var intervalosLivres = new List<IntervaloLivreDTO>();
            foreach (var janela in janelas)
            {
                var inicioJanela = data.Date + janela.HoraInicio;
                var fimJanela = data.Date + janela.HoraFim;

                // Ocupações que intersectam a janela
                var ocupacoes = fretes
                    .Where(f => f.DataHoraInicio < fimJanela && f.DataHoraFimPrevisto > inicioJanela)
                    .Select(f => new
                    {
                        Inicio = f.DataHoraInicio < inicioJanela ? inicioJanela : f.DataHoraInicio,
                        Fim = f.DataHoraFimPrevisto > fimJanela ? fimJanela : f.DataHoraFimPrevisto
                    })
                    .OrderBy(o => o.Inicio)
                    .ToList();

                // Aplicar buffer após cada ocupação
                var ocupacoesComBuffer = new List<(DateTime Inicio, DateTime Fim)>();
                foreach (var o in ocupacoes)
                {
                    ocupacoesComBuffer.Add((o.Inicio, o.Fim));
                    var bufferFim = o.Fim + buffer.Value;
                    if (bufferFim < fimJanela)
                        ocupacoesComBuffer.Add((o.Fim, bufferFim));
                }

                // Gerar intervalos livres percorrendo a janela
                var current = inicioJanela;
                foreach (var ocup in ocupacoesComBuffer)
                {
                    if (ocup.Inicio > current)
                    {
                        intervalosLivres.Add(new IntervaloLivreDTO
                        {
                            Inicio = current,
                            Fim = ocup.Inicio
                        });
                    }
                    current = ocup.Fim;
                }
                if (current < fimJanela)
                {
                    intervalosLivres.Add(new IntervaloLivreDTO
                    {
                        Inicio = current,
                        Fim = fimJanela
                    });
                }
            }

            // Filtrar intervalos com duração mínima (ex: 30 min)
            var duracaoMinima = TimeSpan.FromMinutes(30);
            return intervalosLivres
                .Where(i => i.Fim - i.Inicio >= duracaoMinima)
                .ToList();
        }
    }
}