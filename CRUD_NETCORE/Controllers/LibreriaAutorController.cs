using microservicios.Core.Entities;
using microservicios.Repository;
using Microsoft.AspNetCore.Mvc;

namespace microservicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaAutorController : ControllerBase
    {
        private readonly IMongoRepository<AutorEntity> _autorGenericoRepository;
        public LibreriaAutorController(IMongoRepository<AutorEntity> autorGenericoRepository)
        {
            _autorGenericoRepository = autorGenericoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> Get()
        {
            return Ok(await _autorGenericoRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorEntity>> GetById(string Id)
        {
            var autor = await _autorGenericoRepository.GetById(Id);
            return Ok(autor);
        }

        [HttpPost]
        public async Task Post([FromBody] AutorEntity autor)
        {
            await _autorGenericoRepository.InsertDocument(autor);
        }

        [HttpPut("{id}")]
        public async Task Put(string Id, AutorEntity autor)
        {
            autor.Id = Id;
            await _autorGenericoRepository.UpdateDocument(autor);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string Id) { 
        await _autorGenericoRepository.DeleteById(Id);
        }

        [HttpPost ("pagination")]
        public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPagination(PaginationEntity<AutorEntity> pagination)
        {
            var resultados = await _autorGenericoRepository.PaginationByFilter(
               // filter => filter.Nombre == pagination.Filter,
                pagination 
                );
            return Ok(resultados);
        }


    }
}
