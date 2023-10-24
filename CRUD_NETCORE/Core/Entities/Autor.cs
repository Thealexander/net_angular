using MongoDB.Bson.Serialization.Attributes;

namespace microservicios.Core.entities
{
    public class Autor
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")]
        public string Name { get; set; }

        [BsonElement("apellido")]
        public string Apellido { get; set; }

        [BsonElement("gradoAcademico")]
        public string GradoAcademico { get; set;}
    }
}
