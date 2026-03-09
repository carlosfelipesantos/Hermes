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
            CreateMap<CriarUsuario, Usuario>();
            CreateMap<AtualizarUsuario, Usuario>();
        }
    }
}
