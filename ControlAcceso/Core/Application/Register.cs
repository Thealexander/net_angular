using AutoMapper;
using ControlAcceso.Core.DTO;
using ControlAcceso.Core.Entities;
using ControlAcceso.Core.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ControlAcceso.Core.Application
{
    public class Register
    {
        public class UsuarioRegisterCommand : IRequest<UsuarioDTO> { 
        
        public string Nombre { get; set; }

        public string Apellido { get; set; }    


        public string Username {  get; set; }

        public string Password { get; set; }    

        public string Email { get; set; }

        public string Direccion { get; set; }
        }
        public 


        public class UsuarioRegisterHandler : IRequestHandler<UsuarioRegisterCommand, UsuarioDTO>
        {
            private readonly SeguridadContexto _context;
            private readonly UserManager<Usuario> _usrManager;
            private readonly IMapper _mapper;

            public UsuarioRegisterHandler(SeguridadContexto context, UserManager<Usuario> usrManager, IMapper mapper)
            {
                _context = context;
                _usrManager = usrManager;
                _mapper = mapper;
            }

            public async Task<UsuarioDTO> Handle(UsuarioRegisterCommand request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                {
                    throw new Exception("El Email que intenta utilizar ya se encuentra registrado");
                }
                existe = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if (existe)
                {
                    throw new Exception("Usuario proporcionado ya existe en la BD");
                }

                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    UserName = request.Username,
                    Direccion = request.Direccion
                };

                var resultado = await _usrManager.CreateAsync(usuario, request.Password);
                if (resultado.Succeeded)
                {
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
                    return usuarioDTO;
                }
                throw new Exception("Error al intentar grabaar Nuevo Usuario");
            }
        }

    }
}
