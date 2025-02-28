using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Models {
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Nombre")]
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public required string Nombre { get; set; }

        [BsonElement("Apellidos")]
        [Required(ErrorMessage = "El campo Apellidos es requerido.")]
        public required string Apellidos { get; set; }

        [BsonElement("Cedula")]
        [Required(ErrorMessage = "El campo Cédula es requerido.")]
        public required string Cedula { get; set; }

        [BsonElement("CorreoElectronico")]
        [Required(ErrorMessage = "El campo Correo es requerido.")]
        public required string CorreoElectronico { get; set; }

        [BsonElement("FechaUltimoAcceso")]
        public DateTime FechaUltimoAcceso { get; set; }
    }
}
