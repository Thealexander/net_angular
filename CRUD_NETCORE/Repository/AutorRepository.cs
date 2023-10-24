using microservicios.Core.ContextMongoDB;
using microservicios.Core.entities;
using MongoDB.Driver;

namespace microservicios.Repository
{
    public class AutorRepository : IAutorRepository
    {
        public readonly IAutorContext _autorContext;
        public AutorRepository(IAutorContext autorContext)
        {
            _autorContext = autorContext;
        }
        public async Task<IEnumerable<Autor>> GetAutores()
        {   
            return await _autorContext.Autores.Find(propa => true).ToListAsync();
        }
    }
}
