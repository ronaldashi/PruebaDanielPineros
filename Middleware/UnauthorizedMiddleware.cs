using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class UnauthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // El constructor toma un objeto RequestDelegate para invocar el siguiente middleware en la cadena.

    public async Task Invoke(HttpContext context)
    {
        // Antes de continuar con el siguiente middleware, ejecuta este código.
        await _next(context);

        // Verifica si la respuesta HTTP tiene un código de estado 401 (No autorizado).
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            // Configura el tipo de contenido de la respuesta como JSON.
            context.Response.ContentType = "application/json";

            // Escribe una respuesta JSON personalizada que indica "Usuario no autorizado".
            await context.Response.WriteAsync("{\"message\": \"Usuario no autorizado\"}");
        }
    }
}
