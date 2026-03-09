using AutoMapper;
using Hermes.DTOs.Frete;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class FreteProfile : Profile
    {
        public FreteProfile()
        {
            CreateMap<Frete, FreteDTO>();
            CreateMap<CriarFrete, Frete>();
            CreateMap<AtualizarStatusFrete, Frete>();

        }
    }
}
