using AutoMapper;
using Hermes.DTOs.Usuario;
using Hermes.Entities;

namespace Hermes.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<CriarUsuario, Cliente>();
            CreateMap<CriarUsuario, Usuario>();
            CreateMap<Cliente, UsuarioDTO>();
            CreateMap<AtualizarUsuario, Usuario>();
        }
    }
}
