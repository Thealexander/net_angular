﻿using microservicios.Core;
using microservicios.Core.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

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


        public async Task<IEnumerable<TDocument>> GetAll()
        {
           return  await _collection.Find(p => true ).ToListAsync();  //esto retorna todo registro de la coleccion
        }

        public async Task<TDocument> GetById(string Id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
            return  await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task InsertDocument(TDocument document)
        {
            //document.Id = ObjectId.GenerateNewId().ToString();
            await _collection.InsertOneAsync(document);
           // return CreatedAtAction(nameof(Get), new { id = document.Id }, document);
        }

        public async Task UpdateDocument(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
              await _collection.FindOneAndReplaceAsync(filter, document) ;
        }

        public async Task DeleteById(string Id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, Id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<PaginationEntity<TDocument>> PaginationBy(Expression<Func<TDocument, bool>> filterExpression, PaginationEntity<TDocument> pagination)
        {
            var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
            if (pagination.SortDirection == "desc") {
                sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
                    }

            if (string.IsNullOrEmpty(pagination.Filter))
            {
                pagination.Data = await _collection.Find(p => true)
                        .Sort(sort)
                        .Skip( (pagination.Page-1) * pagination.PageSize)
                        .Limit(pagination.PageSize)
                        .ToListAsync();
            }
            else
            {
                pagination.Data = await _collection.Find(filterExpression)
                       .Sort(sort)
                       .Skip((pagination.Page - 1) * pagination.PageSize)
                       .Limit(pagination.PageSize)
                       .ToListAsync();
            }
            long totalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
            var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocuments / pagination.PageSize)));

            pagination.PageSize = totalPages;

            return pagination;
        }

        public async Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination)
        {
            var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
            if (pagination.SortDirection == "desc")
            {
                sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
            }

            var totalDocuments = 0;

            if (pagination.FilterValue == null)
            {
                pagination.Data = await _collection.Find(p => true)
                        .Sort(sort)
                        .Skip((pagination.Page - 1) * pagination.PageSize)
                        .Limit(pagination.PageSize)
                        .ToListAsync();

                totalDocuments = (await _collection.Find(p => true).ToListAsync()).Count();
            }
            else
            {
                var valueFilter = ".*" + pagination.FilterValue.Valor + ".*";
                var filter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Propiedad,
                    new BsonRegularExpression(valueFilter, "i"));
                pagination.Data = await _collection.Find(filter)
                        .Sort(sort)
                        .Skip((pagination.Page - 1) * pagination.PageSize)
                        .Limit(pagination.PageSize)
                        .ToListAsync();
                totalDocuments = (await _collection.Find(filter).ToListAsync()).Count();
            }

            if (pagination.PageSize != 0 && pagination.PageSize != null)
            {
                var rounded = Math.Ceiling(totalDocuments / Convert.ToDecimal(pagination.PageSize));
                var totalPages = Convert.ToInt32(rounded);
                pagination.PageSize = totalPages;
            }
            else
            {
                // Handle the case where pagination.PageSize is zero or null
                // For example, set a default value or handle it as appropriate.
                // Here, we'll set a default value of 1.
                pagination.PageSize = 1;
            }

            pagination.TotalRows = Convert.ToInt32(totalDocuments);

            return pagination;
        }
    }
}
