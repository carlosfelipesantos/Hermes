using Hermes.Data;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Hermes.Utils;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly HermesBD _context;
        private readonly INotificacaoService _notificacaoService;

        public AvaliacaoService(HermesBD context, INotificacaoService notificacaoService)
        {
            _context = context;
            _notificacaoService = notificacaoService;
        }

        public async Task<Avaliacao> Criar(Avaliacao avaliacao)
        {
            // Busca o frete
            var frete = await _context.Fretes
                .Include(f => f.Avaliacao)
                .FirstOrDefaultAsync(f => f.Id == avaliacao.FreteId);

            if (frete == null)
                throw new NotFoundException($"Frete {avaliacao.FreteId} não encontrado");

            if (frete.ClienteId != avaliacao.ClienteId)
                throw new BusinessException("Você só pode avaliar fretes que solicitou");

            if (frete.Status != StatusFrete.Concluido)
                throw new BusinessException("Frete ainda não foi concluído");

            if (frete.Avaliacao != null)
                throw new ConflictException("Frete já avaliado");

            if (frete.TransportadorId != avaliacao.TransportadorId)
                throw new BusinessException("O transportador informado não corresponde ao frete");

            var transportador = await _context.Transportadores
                .FirstOrDefaultAsync(t => t.Id == avaliacao.TransportadorId);

            if (transportador == null)
                throw new NotFoundException($"Transportador {avaliacao.TransportadorId} não encontrado");

            avaliacao.Frete = frete;
            avaliacao.Transportador = transportador;
            avaliacao.DataAvaliacao = TimeHelper.Now;

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            // Atualizar média e total de avaliações do transportador
            var media = await CalcularMediaTransportador(avaliacao.TransportadorId);
            transportador.AvaliacaoMedia = media;
            transportador.TotalAvaliacoes = await _context.Avaliacoes
                .CountAsync(a => a.TransportadorId == avaliacao.TransportadorId);
            await _context.SaveChangesAsync();

            await _notificacaoService.CriarNotificacao(
                avaliacao.TransportadorId,
                "Você recebeu uma avaliação",
                $"Seu frete #{avaliacao.FreteId} recebeu uma nota de {avaliacao.Nota}",
                TipoNotificacao.AvaliacaoRecebida,
                avaliacao.FreteId
            );

            return avaliacao;
        }

        public async Task<double> CalcularMediaTransportador(int transportadorId)
        {
            var notas = await _context.Avaliacoes
                .Where(a => a.TransportadorId == transportadorId)
                .Select(a => a.Nota)
                .ToListAsync();

            if (!notas.Any())
                return 0;

            return notas.Average();
        }

        public async Task<IEnumerable<Avaliacao>> ListarPorTransportador(int transportadorId)
        {
            var avaliacoes = await _context.Avaliacoes
                .Where(a => a.TransportadorId == transportadorId)
                .Include(a => a.Frete)
                .ToListAsync();

            if (!avaliacoes.Any())
                throw new NotFoundException($"Nenhuma avaliação encontrada para o transportador {transportadorId}");

            return avaliacoes;
        }
    }
}