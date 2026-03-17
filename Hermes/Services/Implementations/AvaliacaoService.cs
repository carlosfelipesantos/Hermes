using Hermes.Data;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly HermesBD _context;

        public AvaliacaoService(HermesBD context)
        {
            _context = context;
        }

        public async Task<Avaliacao> Criar(Avaliacao avaliacao)
        {
            // 1️⃣ Busca o frete
            var frete = await _context.Fretes
                .Include(f => f.Avaliacao)
                .FirstOrDefaultAsync(f => f.Id == avaliacao.FreteId);

            if (frete == null)
                throw new Exception("Frete não encontrado");

            if (frete.Status != Enums.StatusFrete.Concluido)
                throw new Exception("Frete ainda não foi concluído");

            if (frete.Avaliacao != null)
                throw new Exception("Frete já avaliado");

            // 2️⃣ Busca o transportador
            var transportador = await _context.Transportadores
                .FirstOrDefaultAsync(t => t.Id == avaliacao.TransportadorId);

            if (transportador == null)
                throw new Exception("Transportador não encontrado");

            // 3️⃣ Vincula os objetos de navegação
            avaliacao.Frete = frete;
            avaliacao.Transportador = transportador;

            avaliacao.DataAvaliacao = DateTime.Now;

            // 4️⃣ Salva no banco
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

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

            return notas.Average(); //avaerage é um método de extensão do LINQ que calcula a média de colecao de valores
        }



        public async Task<IEnumerable<Avaliacao>> ListarPorTransportador(int transportadorId)
        {
            return await _context.Avaliacoes
             .Where(a => a.TransportadorId == transportadorId)
             .Include(a => a.Frete)
             .ToListAsync();

        }



    }
}
