using System.Security.Cryptography;
using System.Text;

namespace ApiPvtRegiones.Middleware;

public class MiddlewareApiKey(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string NombreHeader = "X-Api-Key";

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // Swagger queda disponible en desarrollo; los endpoints siguen protegidos por API Key.
        if (context.Request.Path.StartsWithSegments("/swagger") && context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            await _next(context);
            return;
        }

        var claveConfigurada = configuration["Seguridad:ApiKey"];
        if (string.IsNullOrWhiteSpace(claveConfigurada) || !context.Request.Headers.TryGetValue(NombreHeader, out var claveRecibida))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { mensaje = "No autorizado." });
            return;
        }

        var esperada = Encoding.UTF8.GetBytes(claveConfigurada);
        var recibida = Encoding.UTF8.GetBytes(claveRecibida.ToString());
        if (esperada.Length != recibida.Length || !CryptographicOperations.FixedTimeEquals(esperada, recibida))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { mensaje = "No autorizado." });
            return;
        }
        await _next(context);
    }
}
