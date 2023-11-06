using AutoMapper;
using ControlAcceso.Core.Entities;

namespace ControlAcceso.Core.DTO
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() {
            CreateMap<Usuario, UsuarioDTO>();
        } 

    }
}
