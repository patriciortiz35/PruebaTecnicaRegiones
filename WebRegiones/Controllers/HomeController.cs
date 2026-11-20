using Microsoft.AspNetCore.Mvc;
using WebRegiones.Services;
namespace WebRegiones.Controllers;
public class HomeController(IServicioRegiones servicio, ILogger<HomeController> logger) : Controller
{
    private readonly IServicioRegiones _servicio = servicio;
    private readonly ILogger<HomeController> _logger = logger;

    /// <summary>
    /// Vista principal Home
    /// </summary>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> Index()
    {
        try { 
            return View(await _servicio.ObtenerRegionesAsync()); 
        }
        catch (Exception ex) 
        { 
            _logger.LogError(ex, "Error al consultar regiones."); 
            TempData["Error"] = "No fue posible conectar con el servicio de regiones."; 
            return View(Array.Empty<WebRegiones.Models.RegionViewModel>()); 
        }
    }
    public IActionResult Error() => View();
}
