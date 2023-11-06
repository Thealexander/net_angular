using AutoMapper;
using ControlAcceso.Core.Entities;
using ControlAcceso.Core.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ControlAcceso.Core.Application.Register;

namespace ControlAcceso.Core.DTO
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set;}
        public string Direccion { get; set; }

    }
    
}
