using ControlAcceso.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ControlAcceso.Core.Persistence
{
    public class SeguridadData
    {
        public static async Task InsertarUsuario(SeguridadContexto context, UserManager<Usuario> usuarioManager) {

            if (!usuarioManager.Users.Any()) {

                var usuario = new Usuario { 
                    Nombre= "Alexander",
                    Apellido = "Gaitan",
                    Direccion ="Av San Juan de Oriente",
                    UserName ="balexg17",
                    Email ="test@testlocal.com"
                };

                await usuarioManager.CreateAsync(usuario, "Password123$");
            }

        }

    }
}
