using Microsoft.AspNetCore.Mvc;
using WebRegiones.Models;
using WebRegiones.Services;
namespace WebRegiones.Controllers;
public class ComunaController(IServicioRegiones servicio, ILogger<ComunaController> logger) : Controller
{
    private readonly IServicioRegiones _servicio = servicio;
    private readonly ILogger<ComunaController> _logger = logger;

    /// <summary>
    /// Llamada Vista Index (principal)
    /// </summary>
    /// <param name="idRegion"></param>
    /// <returns></returns>
    public async Task<IActionResult> Index(int idRegion)
    {
        try { 
            ViewBag.IdRegion = idRegion; 
            return View(await _servicio.ObtenerComunasAsync(idRegion)); 
        }
        catch (Exception ex) { 
            _logger.LogError(ex, "Error al consultar comunas de región {IdRegion}.", idRegion); 
            TempData["Error"] = "No fue posible consultar las comunas."; 
            return View(Array.Empty<ComunaViewModel>()); 
        }
    }

    /// <summary>
    /// Llamada Vista Editar
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <param name="idComuna">int</param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> Editar(int idRegion, int idComuna)
    {
        try { 
            var comuna = await _servicio.ObtenerComunaAsync(idRegion, idComuna); 
            return comuna is null ? NotFound() : View(comuna); 
        }
        catch (Exception ex) { 
            _logger.LogError(ex, "Error al obtener comuna {IdComuna}.", idComuna); 
            TempData["Error"] = "No fue posible obtener la comuna."; 
            return RedirectToAction(nameof(Index), new { idRegion }); 
        }
    }

    /// <summary>
    /// Método post para actualizar datos de una comuna
    /// </summary>
    /// <param name="modelo">ComunaViewModel</param>
    /// <returns>IActionResult</returns>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(ComunaViewModel modelo)
    {
        if (!ModelState.IsValid) 
            return View(modelo);
        try { 
            await _servicio.ActualizarComunaAsync(modelo); 
            TempData["Mensaje"] = "Comuna actualizada correctamente."; 
            return RedirectToAction(nameof(Index), new { idRegion = modelo.IdRegion }); 
        }
        catch (Exception ex) { 
            _logger.LogError(ex, "Error al actualizar comuna {IdComuna}.", modelo.IdComuna); 
            ModelState.AddModelError(string.Empty, "No fue posible actualizar la comuna. Verifique la conexión con la API."); 
            return View(modelo); 
        }
    }
}
