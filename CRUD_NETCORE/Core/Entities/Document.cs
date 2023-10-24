using MongoDB.Bson;
using System;

namespace microservicios.Core.Entities
{
    public class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedDate => Id.CreationTime;
    }
}