using Microsoft.AspNetCore.Mvc;

namespace ApiPvtRegiones.Middleware;

public class MiddlewareExcepciones(RequestDelegate next, ILogger<MiddlewareExcepciones> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<MiddlewareExcepciones> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado. Ruta: {Ruta}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor.",
                Detail = "Se produjo un error al procesar la solicitud."
            });
        }
    }
}
