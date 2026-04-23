using Hermes.Data;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Hermes.Utils;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly HermesBD _context;

        public NotificacaoService(HermesBD context)
        {
            _context = context;
        }

        // Listar notificações de um usuário
        public async Task<List<Notificacao>> ListarNotificacoesPorUsuario(int usuarioId)
        {
            return await _context.Notificacoes
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.DataCriacao)
                .ToListAsync();
        }

        // Marcar notificação como lida
        public async Task<bool> MarcarComoLida(int notificacaoId, int usuarioId)
        {
            var notificacao = await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Id == notificacaoId && n.UsuarioId == usuarioId);

            if (notificacao == null)
                throw new NotFoundException($"Notificação {notificacaoId} não encontrada para o usuário {usuarioId}");

            notificacao.Lida = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // Cria notificação genérica
        public async Task<Notificacao> CriarNotificacao(
            int usuarioId,
            string titulo,
            string mensagem,
            TipoNotificacao tipo,
            int? freteId = null
        )
        {
            // Validar se o usuário existe
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId && u.Ativo);
            if (!usuarioExiste)
                throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado ou inativo");

            var notificacao = new Notificacao
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensagem = mensagem,
                Tipo = tipo,
                FreteId = freteId,
                Lida = false,
                DataCriacao = TimeHelper.Now
            };

            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();

            return notificacao;
        }
    }
}