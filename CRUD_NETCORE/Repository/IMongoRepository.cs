using microservicios.Core.Entities;

namespace microservicios.Repository
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> GetAll();


    }
}
