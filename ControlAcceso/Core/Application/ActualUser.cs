using AutoMapper;
using ControlAcceso.Core.DTO;
using ControlAcceso.Core.Entities;
using ControlAcceso.Core.JwtLogic;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ControlAcceso.Core.Application
{
    public class ActualUser
    {

        public class UsuarioActualCommnad : IRequest<UsuarioDTO> { }

        public class ActualUsuarioHandler : IRequestHandler<UsuarioActualCommnad, UsuarioDTO>
        {
            private readonly  UserManager<Usuario> _userManager;
            private readonly IUsuarioSesion _usuarioSesion;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IMapper _mapper;


            public ActualUsuarioHandler(UserManager<Usuario> userManager, IUsuarioSesion usuarioSesion, IJwtGenerator jwtGenerator, IMapper mapper) {
            
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _usuarioSesion = usuarioSesion;
                _mapper = mapper;
                
            }

            public  async Task<UsuarioDTO> Handle(UsuarioActualCommnad request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.GetUsuarioSesion());

                if(usuario != null) {

                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
                    usuarioDTO.Token = _jwtGenerator.CreateToken(usuario);
                    return usuarioDTO;
                }
                throw new Exception("Usuario No encontrado");
            }
        }


    }
}
