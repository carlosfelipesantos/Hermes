using Hermes.Entities;
using Hermes.Enums;

namespace Hermes.Services.Interfaces
{
    public interface INotificacaoService
    {
        Task<Notificacao> CriarNotificacao(
            int usuarioId,
            string titulo,
            string mensagem,
            TipoNotificacao tipo,
            int? freteId = null
        );
    }
}
