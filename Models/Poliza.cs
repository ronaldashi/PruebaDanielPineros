using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace PruebaDanielPineros.Models
{
    public class Poliza
    {
        // Identificador único de la póliza en la base de datos MongoDB.
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Número de la póliza.
        public string NumberPoliza { get; set; } = null!;

        // Nombre del cliente de la póliza.
        public string NameClient { get; set; } = null!;

        // Número de identificación del cliente.
        public double Identification { get; set; }

        // Fecha de nacimiento del cliente.
        public DateTime Birthdate { get; set; }

        // Fecha de inicio de vigencia de la póliza con atributos de formato y validación.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Inicio de Vigencia")]
        [Required(ErrorMessage = "La fecha de inicio de vigencia es obligatoria.")]
        [DateNotInPast(ErrorMessage = "La fecha de inicio de vigencia debe estar vigente.")]
        public DateTime DateCreatePoliza { get; set; }

        // Fecha de fin de vigencia de la póliza con atributos de formato y validación.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Fin de Vigencia")]
        [Required(ErrorMessage = "La fecha de fin de vigencia es obligatoria.")]
        [DateNotInPast(ErrorMessage = "La fecha de fin de vigencia debe estar vigente.")]
        public DateTime DateEndPoliza { get; set; }

        // Cobertura de la póliza.
        public string Coverage { get; set; } = null!;

        // Valor máximo de la póliza con validación de rango.
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
        public double MaximumValuePoliza { get; set; }

        // Nombre del plan de la póliza.
        public string PolizaPlanName { get; set; } = null!;

        // Ciudad del cliente.
        public string CityClient { get; set; } = null!;

        // Dirección del cliente.
        public string AddressClient { get; set; } = null!;

        // Placa del vehículo asegurado.
        public string LicensePlate { get; set; } = null!;

        // Modelo del vehículo asegurado.
        public string ModelVehicle { get; set; } = null!;

        // Indica si se ha inspeccionado el vehículo.
        public Boolean InspectionVehicle { get; set; }

        // Método que valida si la fecha de fin de vigencia es posterior a la fecha de inicio de vigencia.
        public Boolean ValidateDates(DateTime dateIni, DateTime dateFin)
        {
            return dateFin > dateIni;
        }
    }
}
