using Hermes.Data;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class NotificacaoService: INotificacaoService
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
                Lida = false,
                DataCriacao = DateTime.Now
            };

            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();

            return notificacao;
        }
    }
}