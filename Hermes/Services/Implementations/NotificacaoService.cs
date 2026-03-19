using Hermes.Data;
using Hermes.Entities;
using Hermes.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class NotificacaoService
    {
        private readonly HermesBD _context;

        public NotificacaoService(HermesBD context)
        {
            _context = context;
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
            var notificacao = new Notificacao
            {
                UsuarioId = usuarioId,
                Titulo = titulo,
                Mensagem = mensagem,
                Tipo = tipo,
                FreteId = freteId,
                Lida = false
            };

            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();

            return notificacao;
        }
    }
}