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
        public async Task<IEnumerable<Frete>> BuscarFretesParaTransportador(int transportadorId)
        {
            var transportador = await _context.Transportadores
                .FirstOrDefaultAsync(t => t.Id == transportadorId);

            if (transportador == null)
                return new List<Frete>();

            var fretes = await _context.Fretes
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();

            var fretesProximos = fretes
                .Where(f =>
                    CalcularDistancia(
                        transportador.Latitude,
                        transportador.Longitude,
                        f.LatitudeOrigem,
                        f.LongitudeOrigem
                    ) <= 20
                );

            return fretesProximos;
        }

        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371;

            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) *
                Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }



        public async Task<bool> AceitarFrete(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(freteId);

            if (frete == null)
                return false;

            if (frete.TransportadorId != null)
                return false;

            if (frete.Status != StatusFrete.Pendente)
                return false;

            frete.TransportadorId = transportadorId;
            frete.Status = StatusFrete.Aceito;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Frete>> ListarPorCidade(string cidade)
        {
            return await _context.Fretes
                .Where(f => f.CidadeOrigem.ToLower() == cidade.ToLower()) //toLower para comparação case-insensitive
                .ToListAsync();
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

        public async Task<IEnumerable<Frete>> ListarPorCliente(int clienteId)
        {
            return await _context.Fretes
                .Where(f => f.ClienteId == clienteId)
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .ToListAsync();
        }

        public async Task<IEnumerable<Frete>> ListarPorTransportador(int transportadorId)
        {
            return await _context.Fretes
                .Where(f => f.TransportadorId == transportadorId)
                .Include(f => f.Transportador)
                .ToListAsync();
        }

        public async Task<IEnumerable<Frete>> ListarDisponiveis()
        {
            return await _context.Fretes
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();
        }

        public async Task<bool> AtualizarStatus(int id, StatusFrete status)
        {
            var frete = await _context.Fretes.FindAsync(id);

            if(frete  == null)
                return false;

            frete.Status = status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var frete = await _context.Fretes.FindAsync(id);

            if (frete == null)
                return false;

            _context.Fretes.Remove(frete);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
