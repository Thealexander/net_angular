using microservicios.Core;
using microservicios.Core.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace microservicios.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IOptions<MongoSettings> options) {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.Database);
        
                _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }
        private protected string  GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }


        public IQueryable<TDocument> GetAll()
        {
           return _collection.AsQueryable();  //esto retorna todo registro de la coleccion
        }
    }
}
