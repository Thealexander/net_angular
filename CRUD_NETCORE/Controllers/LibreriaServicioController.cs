using microservicios.Core.entities;
using microservicios.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microservicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaServicioController : ControllerBase
    {
        private readonly IAutorRepository _autorRepository;

        public LibreriaServicioController(IAutorRepository autorRepository)
        {
            _autorRepository = autorRepository;
        }

        [HttpGet("autores")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores() 
        { 
            var autores = await _autorRepository.GetAutores();
            return Ok(autores);
        
        }
    }
}
