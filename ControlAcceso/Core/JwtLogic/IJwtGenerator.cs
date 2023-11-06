using ControlAcceso.Core.Entities;

namespace ControlAcceso.Core.JwtLogic
{
    public interface IJwtGenerator
    {

        string CreateToken(Usuario usuario);
    }
}
