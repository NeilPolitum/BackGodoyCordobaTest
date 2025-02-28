using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace UserApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Nombre")]
        public required string Nombre { get; set; }

        [BsonElement("Apellidos")]
        public required string Apellidos { get; set; }

        [BsonElement("Cedula")]
        public required string Cedula { get; set; }

        [BsonElement("CorreoElectronico")]
        public required string CorreoElectronico { get; set; }

        [BsonElement("FechaUltimoAcceso")]
        public DateTime FechaUltimoAcceso { get; set; }
    }
}