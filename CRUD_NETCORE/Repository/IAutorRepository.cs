using microservicios.Core.entities;

namespace microservicios.Repository
{
    public interface IAutorRepository
    {
        Task<IEnumerable<Autor>> GetAutores();
    }
}
