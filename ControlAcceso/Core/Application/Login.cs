using AutoMapper;
using ControlAcceso.Core.DTO;
using ControlAcceso.Core.Entities;
using ControlAcceso.Core.JwtLogic;
using ControlAcceso.Core.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ControlAcceso.Core.Application
{
    public class Login
    {
        public class UsuarioLoginCommand : IRequest<UsuarioDTO> { 
        
        public string Email{ get; set; }
            public string Password { get; set; }
        }
        public class UsuarioLoginValidation : AbstractValidator<UsuarioLoginCommand> {

            public UsuarioLoginValidation() {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class UsuarioLoginHandler : IRequestHandler<UsuarioLoginCommand, UsuarioDTO>
        {
            private readonly SeguridadContexto _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly SignInManager<Usuario> _signInManager;

            public UsuarioLoginHandler(SeguridadContexto context, UserManager<Usuario> userManager, IMapper mapper,
                IJwtGenerator jwtGenerator, SignInManager<Usuario> signInManager)
            { 
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
            }

            public async Task<UsuarioDTO> Handle(UsuarioLoginCommand request, CancellationToken cancellationToken)
            {
              var usuario = await _userManager.FindByEmailAsync(request.Email);

                if (usuario == null) {

                    throw new Exception("Ese usuario no existe");
                    
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                if (resultado.Succeeded) {
                var usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
                    usuarioDTO.Token = _jwtGenerator.CreateToken(usuario);
                    return usuarioDTO;
                }
                throw new Exception("Login Incorrecto");

            }
        }



    }
}
