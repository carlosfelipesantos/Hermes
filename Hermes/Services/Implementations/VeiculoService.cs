using Hermes.Data;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class VeiculoService : IVeiculoService
    {
        private readonly HermesBD _context;

        public VeiculoService(HermesBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Veiculo>> Listar()
        {
            return await _context.Veiculos
               .Include(v => v.Transportador)
               .ToListAsync();
        }
        public async Task<Veiculo> Criar(Veiculo veiculo)
        {
            veiculo.DataCadastro = DateTime.Now;

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return veiculo;
        }


        public async Task<Veiculo> BuscarPorId(int id)
        {
            return await _context.Veiculos
                .Include(v => v.Transportador)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task Atualizar(Veiculo veiculo)
        {
            _context.Veiculos.Update(veiculo);
            await _context.SaveChangesAsync();
        }



        public async Task<bool> Deletar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                return false;

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
