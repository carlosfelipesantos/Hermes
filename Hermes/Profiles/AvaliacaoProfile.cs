using AutoMapper;
using Hermes.DTOs.Avaliacao;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class AvaliacaoProfile : Profile
    {
        public AvaliacaoProfile()
        {
            // Avaliacao -> AvaliacaoDTO 
            CreateMap<Avaliacao, AvaliacaoDTO>();

            // CriarAvaliacao -> Avaliacao 
            CreateMap<CriarAvaliacao, Avaliacao>()
                .ForMember(dest => dest.TransportadorId, opt => opt.MapFrom(src => src.TransportadorId));
        }
    }
}