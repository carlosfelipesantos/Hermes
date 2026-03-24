using AutoMapper;
using Hermes.DTOs.Frete;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class FreteProfile : Profile
    {
        public FreteProfile()
        {

            CreateMap<Frete, FretePublicoDTO>()
                .ForMember(dest => dest.Origem,
                    opt => opt.MapFrom(src =>
                        src.SitioOrigem
                            ? $"Sítio - {src.DescricaoOrigem}"
                            : $"{src.CidadeOrigem} - {src.BairroOrigem}"))

                    .ForMember(dest => dest.Destino,
                        opt => opt.MapFrom(src =>
                            src.SitioDestino
                                ? $"Sítio - {src.DescricaoDestino}"
                                : $"{src.CidadeDestino} - {src.BairroDestino}"))

                    .ForMember(dest => dest.NomeTransportador,
                        opt => opt.MapFrom(src =>
                            src.Transportador != null ? src.Transportador.Nome : null));

            CreateMap<Frete, FreteDTO>()
                // ORIGEM (COM SITIO)
                .ForMember(dest => dest.Origem,
                    opt => opt.MapFrom(src =>
                        src.SitioOrigem
                            ? $" Sítio - {src.DescricaoOrigem}"
                            : $"{src.CidadeOrigem} - {src.BairroOrigem}"))

                // DESTINO (COM SITIO)
                .ForMember(dest => dest.Destino,
                    opt => opt.MapFrom(src =>
                        src.SitioDestino
                            ? $" Sítio - {src.DescricaoDestino}"
                            : $"{src.CidadeDestino} - {src.BairroDestino}"))

                // NOMES
                .ForMember(dest => dest.NomeCliente,
                    opt => opt.MapFrom(src => src.Cliente.Nome))

                .ForMember(dest => dest.NomeTransportador,
                    opt => opt.MapFrom(src =>
                        src.Transportador != null ? src.Transportador.Nome : null))

                // TELEFONE
                .ForMember(dest => dest.TelefoneTransportador,
                    opt => opt.MapFrom(src =>
                        src.Transportador != null ? src.Transportador.Telefone : null))

                // MENSAGEM WHATSAPP
                .ForMember(dest => dest.MensagemWhatsapp,
                    opt => opt.MapFrom(src =>
                        $"Olá, vi seu frete no Hermes." +
                        $" Origem: {(src.SitioOrigem ? " Sítio - " + src.DescricaoOrigem : src.CidadeOrigem + " - " + src.BairroOrigem)}" +
                        $" Destino: {(src.SitioDestino ? " Sítio - " + src.DescricaoDestino : src.CidadeDestino + " - " + src.BairroDestino)}" +
                        $" Carga: {src.DescricaoCarga}" +
                        $" Valor: R$ {src.Valor}"
                    ))

                //FretePublicoDTO


                // CAMPOS NOVOS
                .ForMember(dest => dest.SitioOrigem, opt => opt.MapFrom(src => src.SitioOrigem))
                .ForMember(dest => dest.DescricaoOrigem, opt => opt.MapFrom(src => src.DescricaoOrigem))
                .ForMember(dest => dest.SitioDestino, opt => opt.MapFrom(src => src.SitioDestino))
                .ForMember(dest => dest.DescricaoDestino, opt => opt.MapFrom(src => src.DescricaoDestino))
                .ForMember(dest => dest.DistanciaExtra, opt => opt.MapFrom(src => src.DistanciaExtra));

            // CRIAR FRETE
            CreateMap<CriarFrete, Frete>();

            // ATUALIZAR STATUS
            CreateMap<AtualizarStatusFrete, Frete>();

            
        }
    }
}