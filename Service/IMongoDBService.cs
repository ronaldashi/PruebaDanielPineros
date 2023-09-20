using System.Collections.Generic;
using System.Threading.Tasks;
using PruebaDanielPineros.Models;

namespace PruebaDanielPineros.Services
{
    public interface IMongoDBService
    {
        // Método para crear una nueva póliza en la base de datos.
        Task CreateAsync(Poliza poliza);

        // Método para obtener una lista de pólizas desde la base de datos.
        Task<List<Poliza>> GetAsync();

        // Método para buscar una póliza por placa de vehículo y número de póliza.
        Poliza BuscarPolizaPorPlacaYNumero(string placaVehiculo, string numeroPoliza);

        // Método para eliminar una póliza de la base de datos por su ID.
        Task DeleteAsync(string id);
    }
}
