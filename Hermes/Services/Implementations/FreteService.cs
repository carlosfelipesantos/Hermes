using Hermes.Data;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class FreteService : IFreteService
    {
        private readonly HermesBD _context;

        public FreteService(HermesBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Frete>> Listar()
        {
            return await _context.Fretes
                 .Include(f => f.Cliente)
                 .Include(f => f.Transportador)
                 .ToListAsync();
        }

        public async Task<Frete> BuscarPorId(int id)
        {
            return await _context.Fretes
                .Include (f => f.Cliente)
                .Include(f => f.Transportador)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Frete> Criar(Frete frete)
        {
            frete.DataSolicitacao = DateTime.Now;
            frete.Status = StatusFrete.Pendente;

            _context.Fretes.Add(frete);
            await _context.SaveChangesAsync();

            return frete;
        }



        public async Task<bool> AceitarFrete(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(freteId);

            if(frete ==null )
                return false;

            frete.TransportadorId = transportadorId;
            frete.Status = StatusFrete.Aceito;
            await _context.SaveChangesAsync(); 

            return true;
        }

       
        
        public async Task<bool> FinalizarFrete(int id)
        {
            var frete = await _context.Fretes.FindAsync(id);

            if(frete ==null )
                return false;

            frete.Status = StatusFrete.Concluido;
            frete.DataConclusao = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }

      
    }
}
