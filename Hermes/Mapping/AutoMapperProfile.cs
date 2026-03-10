using AutoMapper;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;

namespace Hermes.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CriarVeiculo, Veiculo>()
                // Mapear explicitamente o FK
                .ForMember(dest => dest.TransportadorId, opt => opt.MapFrom(src => src.TransportadorId));

            CreateMap<Veiculo, Hermes.DTOs.Veiculo.VeiculoDTO>();
        }
    }
}