namespace ControlAcceso.Core.JwtLogic
{
    public class UsuarioSesion: IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioSesion(IHttpContextAccessor httpContextAccesor) {
        _httpContextAccessor = httpContextAccesor;
        }

        public string GetUsuarioSesion()
        {

            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;

            return userName;

        }
    
    



    }
}
