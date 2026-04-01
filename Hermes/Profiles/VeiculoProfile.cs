using AutoMapper;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class VeiculoProfile : Profile 
    {
        public VeiculoProfile() 
        {
            CreateMap<Veiculo, VeiculoDTO>(); 
            CreateMap<CriarVeiculo, Veiculo>();
            CreateMap<AtualizarVeiculo, Veiculo>();

        }


    }
}
