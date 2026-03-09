using AutoMapper;
using Hermes.DTOs.Notificacao;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class NotificacaoProfile : Profile
    {
        public NotificacaoProfile()
        {
            CreateMap<Notificacao, NotificacaoDTO>();
        }
    }
}
