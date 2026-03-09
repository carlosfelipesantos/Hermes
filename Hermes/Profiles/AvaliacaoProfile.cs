using AutoMapper;
using Hermes.DTOs.Avaliacao;

namespace Hermes.Profiles
{
    public class AvaliacaoProfile : Profile
    {
        public AvaliacaoProfile()
        {
            CreateMap<AvaliacaoProfile, AvaliacaoDTO>();
            CreateMap<CriarAvaliacao, AvaliacaoDTO>();
        }
    }
}
