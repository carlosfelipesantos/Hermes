using AutoMapper;
using Hermes.DTOs.Veiculo;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class VeiculoProfile : Profile //heranca do automapper, ou seja veiculo profile mapeia veiculo e veiculo DTO
    {
        public VeiculoProfile() //ctor do veiculo profile, ou seja, quando for criar um veiculo profile, ele vai fazer o mapeamento entre veiculo e veiculo DTO automaticamente.
        {
            CreateMap<Veiculo, VeiculoDTO>(); //mapeamento entre veiculo e veiculo DTO, ou seja, quando for converter de um para outro, ele vai fazer o mapeamento automaticamente).
            CreateMap<CriarVeiculo, Veiculo>();
            CreateMap<AtualizarVeiculo, Veiculo>();

        }


    }
}
