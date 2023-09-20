using PruebaDanielPineros.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaDanielPineros.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<Poliza> _polizaCollection;

        // Constructor sin parámetros para Moq u otros usos.
        public MongoDBService()
        {
            // Constructor vacío utilizado en ciertos contextos.
        }

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            // Constructor utilizado para la inyección de dependencias de configuración.

            // Crea un cliente MongoDB y una conexión a la base de datos utilizando la configuración proporcionada.
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            // Obtiene una colección de pólizas de la base de datos.
            _polizaCollection = database.GetCollection<Poliza>(mongoDBSettings.Value.CollectionName);

            Console.WriteLine("Conexión a MongoDB establecida con éxito.");
        }

        public virtual async Task CreateAsync(Poliza poliza)
        {
            // Inserta una nueva póliza en la colección de MongoDB.
            await _polizaCollection.InsertOneAsync(poliza);
            return;
        }

        public virtual async Task<List<Poliza>> GetAsync()
        {
            // Obtiene una lista de todas las pólizas almacenadas en la colección.
            return await _polizaCollection.Find(new BsonDocument()).ToListAsync();
        }

        public virtual Poliza BuscarPolizaPorPlacaYNumero(string placaVehiculo, string numeroPoliza)
        {
            // Busca una póliza por placa de vehículo o número de póliza en la colección.
            return _polizaCollection.Find(p =>
                p.LicensePlate == placaVehiculo || p.NumberPoliza == numeroPoliza)
                .FirstOrDefault();
        }

        public async Task DeleteAsync(string id)
        {
            // Elimina una póliza de la colección por su ID.
            FilterDefinition<Poliza> filter = Builders<Poliza>.Filter.Eq("Id", id);
            await _polizaCollection.DeleteOneAsync(filter);
            return;
        }
    }
}
