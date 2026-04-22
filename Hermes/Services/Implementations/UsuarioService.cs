using Hermes.Data;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Hermes.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HermesBD _context;

        public UsuarioService(HermesBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> Listar()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> BuscarPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> Criar(Usuario usuario)
        {
            
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExiste)
                throw new Exception("Email já está em uso");

           
            var telefoneExiste = await _context.Usuarios
                .AnyAsync(u => u.DDD == usuario.DDD && u.Telefone == usuario.Telefone);
            if (telefoneExiste)
                throw new Exception("Telefone já está em uso");

            // Gera o hash da senha usando BCrypt
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            usuario.DataCadastro = DateTime.Now;
            usuario.Ativo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> Autenticar(string email, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return null;

            // Verifica a senha com BCrypt
            bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);
            if (!senhaValida)
                return null;

            return usuario;
        }

        public async Task<bool> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}