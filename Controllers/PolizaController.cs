using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaDanielPineros.Models;
using PruebaDanielPineros.Services;
using System;

using System.Text;
using System.Threading.Tasks;
using PruebaDanielPineros.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;
using System.Net;

namespace PruebaDanielPineros.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class PolizaController : Controller
    {
        private readonly MongoDBService _mongoDBService;
        private readonly string secretKey;

        public PolizaController(MongoDBService mongoDBService, IConfiguration config)
        {
            _mongoDBService = mongoDBService;

            // Se obtiene la clave secreta desde la configuración de la aplicación.
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpPost]
        [Route("Valid")]
        public IActionResult Valid([FromBody] UserLogin request)
        {
            // Comprueba si el correo electrónico y la contraseña coinciden (esto es una simplificación).
            if (request.Email == "danielpineros@gmail.com" && request.Password == "daniel123")
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Email));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                // Devuelve un token JWT si la autenticación es exitosa.
                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
            }
            else
            {
                // Devuelve un código de error si la autenticación falla.
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Poliza>>> Get()
        {
            // Obtiene una lista de pólizas (requiere autorización).
            var polizas = await _mongoDBService.GetAsync();
            return Ok(polizas);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Poliza poliza)
        {
            if (ModelState.IsValid)
            {
                // Valida las fechas de la póliza antes de crearla.
                Boolean isValid = poliza.ValidateDates(poliza.DateCreatePoliza, poliza.DateEndPoliza);
                if (!isValid)
                {
                    return BadRequest("La fecha final debe ser mayor a la Fecha de creación");
                }

                // Crea una nueva póliza y la guarda en la base de datos.
                await _mongoDBService.CreateAsync(poliza);
                return CreatedAtAction(nameof(Get), new { id = poliza.Id }, poliza);
            }

            // Devuelve un error si el modelo no es válido.
            return BadRequest(ModelState);
        }

        [HttpGet("buscar")]
        [Authorize]
        public IActionResult BuscarPoliza(string placaVehiculo, string numeroPoliza)
        {
            // Busca una póliza por placa de vehículo y número de póliza.
            var poliza = _mongoDBService.BuscarPolizaPorPlacaYNumero(placaVehiculo, numeroPoliza);
            if (poliza == null)
            {
                return NotFound("No se encontró la póliza.");
            }

            // Convierte el resultado en un objeto DTO y lo devuelve.
            var polizaDto = new Poliza
            {
                Id = poliza.Id,
                NumberPoliza = poliza.NumberPoliza,
                NameClient = poliza.NameClient,
                Identification = poliza.Identification,
                Birthdate = poliza.Birthdate,
                DateCreatePoliza = poliza.DateCreatePoliza,
                DateEndPoliza = poliza.DateEndPoliza,
                Coverage = poliza.Coverage,
                MaximumValuePoliza = poliza.MaximumValuePoliza,
                PolizaPlanName = poliza.PolizaPlanName,
                CityClient = poliza.CityClient,
                AddressClient = poliza.AddressClient,
                LicensePlate = poliza.LicensePlate,
                ModelVehicle = poliza.ModelVehicle,
                InspectionVehicle = poliza.InspectionVehicle
                // Agrega más propiedades al DTO según corresponda.
            };

            return Ok(polizaDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (ObjectId.TryParse(id, out _))
            {
                // Elimina una póliza por su ID.
                await _mongoDBService.DeleteAsync(id);
                return Ok("Póliza eliminada correctamente");
            }

            // Devuelve un error si el formato del ID es incorrecto.
            return NotFound("No se encontró la póliza.");
        }
    }
}
