using System;

namespace PruebaDanielPineros.Models
{
    public class MongoDBSettings
    {
        // Propiedad que representa la cadena de conexión a la base de datos MongoDB.
        public string ConnectionURI { get; set; } = null!;

        // Propiedad que representa el nombre de la base de datos MongoDB.
        public string DatabaseName { get; set; } = null!;

        // Propiedad que representa el nombre de la colección en la base de datos MongoDB.
        public string CollectionName { get; set; } = null!;
    }
}
