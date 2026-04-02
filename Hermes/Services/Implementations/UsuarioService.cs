using Hermes.Data;
using Hermes.DTOs.Usuario;
using Hermes.Entities;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> Criar(Usuario usuario)
        {
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExiste)
                throw new Exception("Email ja esta em uso");

            var telefoneExiste = await _context.Usuarios.AnyAsync(u => u.Telefone == usuario.Telefone);
            if (telefoneExiste)
                throw new Exception("Telefone ja esta em uso");

            usuario.DataCadastro = DateTime.Now;
            usuario.Ativo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> Autenticar(string email, string senha)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
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

