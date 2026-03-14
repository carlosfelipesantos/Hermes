using AutoMapper;
using Hermes.DTOs.Avaliacao;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class AvaliacaoProfile : Profile
    {
        public AvaliacaoProfile()
        {
            CreateMap<Avaliacao, AvaliacaoDTO>();
            CreateMap<CriarAvaliacao, AvaliacaoDTO>();
        }
    }
}
