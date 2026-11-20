using System.Security.Cryptography;
using System.Text;

namespace ApiPvtRegiones.Middleware;

public class MiddlewareApiKey(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string NombreHeader = "X-Api-Key";

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration,
    ILogger<MiddlewareApiKey> logger)
    {
        // Swagger queda disponible en desarrollo; los endpoints siguen protegidos por API Key.
        if (context.Request.Path.StartsWithSegments("/swagger") && context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            await _next(context);
            return;
        }

        var apiKeyEntorno = Environment.GetEnvironmentVariable("ApiKey__Key");

        //descomenta para diagnosticar la variable de entorno
        //logger.LogWarning(
        //    "Diagnóstico entorno: VariableExiste={Existe}, Longitud={Longitud}",
        //    !string.IsNullOrWhiteSpace(apiKeyEntorno),
        //    apiKeyEntorno?.Length ?? 0
        //);

        var claveConfigurada = configuration["ApiKey:Key"];
        var tieneConfiguracion = !string.IsNullOrWhiteSpace(claveConfigurada);

        var tieneHeader = context.Request.Headers.TryGetValue(
            NombreHeader, out var claveRecibida);

        //descomenta para diagnosticar la configuración y el header recibido
        //logger.LogWarning(
        //"Diagnóstico API Key: Configurada={Configurada}, LongitudConfigurada={LongitudConfigurada}, HeaderRecibido={HeaderRecibido}, LongitudRecibida={LongitudRecibida}",
        //tieneConfiguracion,
        //claveConfigurada?.Length ?? 0,
        //tieneHeader,
        //claveRecibida.ToString().Length
        //);

        if (!tieneConfiguracion || !tieneHeader)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { mensaje = "No autorizado." });
            return;
        }

        var esperada = Encoding.UTF8.GetBytes(claveConfigurada!);
        var recibida = Encoding.UTF8.GetBytes(claveRecibida.ToString());

        if (esperada.Length != recibida.Length ||
            !CryptographicOperations.FixedTimeEquals(esperada, recibida))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { mensaje = "No autorizado." });
            return;
        }

        await _next(context);
    }
}
