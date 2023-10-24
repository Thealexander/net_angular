using microservicios.Core.entities;
using MongoDB.Driver;

namespace microservicios.Core.ContextMongoDB
{
    public interface IAutorContext
    {
        IMongoCollection<Autor> Autores { get; }
    }
}
