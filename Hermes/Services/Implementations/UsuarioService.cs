using BCrypt.Net;
using Hermes.Data;
using Hermes.Entities;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Hermes.Utils;
using Microsoft.EntityFrameworkCore;

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
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                throw new NotFoundException($"Usuário {id} não encontrado");

            return usuario;
        }

        public async Task<Usuario> Criar(Usuario usuario)
        {
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExiste)
                throw new BusinessException("Email já está em uso");

            var telefoneExiste = await _context.Usuarios
                .AnyAsync(u => u.DDD == usuario.DDD && u.Telefone == usuario.Telefone);
            if (telefoneExiste)
                throw new BusinessException("Telefone já está em uso");

            // Gera o hash da senha usando BCrypt
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            usuario.DataCadastro = TimeHelper.Now;
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
                throw new NotFoundException("Usuário não encontrado");

            // Verifica a senha com BCrypt
            bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);
            if (!senhaValida)
                throw new BusinessException("Senha inválida");

            if (!usuario.Ativo)
                throw new BusinessException("Conta desativada. Entre em contato com o suporte.");

            return usuario;
        }

        public async Task<bool> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                throw new NotFoundException($"Usuário {id} não encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}