using Hermes.Data;
using Hermes.Entities;
using Hermes.Services.Interfaces;

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
            avaliacao.DataAvaliacao = DateTime.Now;

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            return avaliacao;
        }
    }
}
