using Hermes.Data;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly HermesBD _context;

        public AdminService(HermesBD context)
        {
            _context = context;
        }

        public async Task<(List<Usuario> data, int total)> ListarUsuarios(int page, int pageSize)
        {
            var query = _context.Usuarios.AsNoTracking();
            var total = await query.CountAsync();
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, total);
        }

        public async Task<(List<Frete> data, int total)> ListarFretes(int page, int pageSize)
        {
            var query = _context.Fretes
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Include(f => f.Transportador);
            var total = await query.CountAsync();
            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, total);
        }

        public async Task<bool> AtualizarStatusUsuario(int id, bool ativo)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            usuario.Ativo = ativo;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}