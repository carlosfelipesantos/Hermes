using AutoMapper;
using Hermes.DTOs.Frete;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class FreteProfile : Profile
    {
        public FreteProfile()
        {
            CreateMap<Frete, FreteDTO>()
                .ForMember(dest => dest.Origem,
                    opt => opt.MapFrom(src => $"{src.CidadeOrigem} - {src.BairroOrigem}"))
                .ForMember(dest => dest.Destino,
                    opt => opt.MapFrom(src => $"{src.CidadeDestino} - {src.BairroDestino}"))
                .ForMember(dest => dest.NomeCliente,
                    opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.NomeTransportador,
                    opt => opt.MapFrom(src => src.Transportador != null ? src.Transportador.Nome : null));

            CreateMap<CriarFrete, Frete>();

            CreateMap<AtualizarStatusFrete, Frete>();
        }
    }
}