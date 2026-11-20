using System.Xml.Linq;
using AccesoDatos.Interfaces;
using ApiPvtRegiones.Models;
using ApiPvtRegiones.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiPvtRegiones.Controllers;

[ApiController]
[Route("api/region")]
[Produces("application/json")]
public class RegionController(IRegionRepositorio regionRepositorio, IComunaRepositorio comunaRepositorio, 
    ILogger<RegionController> logger, Utilitario utilitario) : ControllerBase
{
    private readonly IRegionRepositorio _regionRepositorio = regionRepositorio;
    private readonly IComunaRepositorio _comunaRepositorio = comunaRepositorio;
    private readonly ILogger<RegionController> _logger = logger;
    private readonly Utilitario _utilitario = utilitario;

    /// <summary>
    /// retorna la lista de regiones disponibles
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet]
    public async Task<IActionResult> ObtenerRegiones()
        => Ok(await _regionRepositorio.ObtenerRegionesAsync());

    /// <summary>
    /// Obtiene datos de una región específica según su identificador
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{idRegion:int}")]
    public async Task<IActionResult> ObtenerRegion(int idRegion)
    {
        if (idRegion <= 0) 
            return BadRequest(new { mensaje = "El identificador de región debe ser mayor a cero." });
        
        var region = await _regionRepositorio.ObtenerRegionAsync(idRegion);

        return region is null ? NotFound(new { mensaje = "Región no encontrada." }) : Ok(region);
    }

    /// <summary>
    /// Obtiene datos de las comunas de una región específica según su identificador
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{idRegion:int}/comuna")]
    public async Task<IActionResult> ObtenerComunas(int idRegion)
    {
        if (idRegion <= 0) 
            return BadRequest(new { mensaje = "Identificador de región inválido." });

        if (await _regionRepositorio.ObtenerRegionAsync(idRegion) is null) 
            return NotFound(new { mensaje = "Región no encontrada." });
        
        return Ok((await _comunaRepositorio.ObtenerComunasAsync(idRegion)).Select(_utilitario.MapearComuna));
    }

    /// <summary>
    /// Obtiene datos de una comuna específica de una región según sus IDs región y comuna. 
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <param name="idComuna">int</param>
    /// <returns>IActionResult</returns>
    [HttpGet("{idRegion:int}/comuna/{idComuna:int}")]
    public async Task<IActionResult> ObtenerComuna(int idRegion, int idComuna)
    {
        if (idRegion <= 0 || idComuna <= 0) 
            return BadRequest(new { mensaje = "Identificadores inválidos." });
        
        var comuna = await _comunaRepositorio.ObtenerComunaAsync(idRegion, idComuna);

        return comuna is null ? NotFound(new { mensaje = "Comuna no encontrada para la región indicada." }) : Ok(_utilitario.MapearComuna(comuna));
    }

    /// <summary>
    /// Actualiza los datos de una comuna específica de una región.
    /// Recibe parámetros de la comuna a actualizar en el cuerpo de la solicitud.
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <param name="solicitud">ActualizarComunaRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost("{idRegion:int}/comuna")]
    public async Task<IActionResult> ActualizarComuna(int idRegion, [FromBody] ActualizarComunaRequest solicitud)
    {       
        if (idRegion <= 0)
            return BadRequest(new { success = false, mensaje = "Identificador de región inválido." });

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var comuna = await _comunaRepositorio.ObtenerComunaAsync(idRegion, solicitud.IdComuna);
        if (comuna is null)
            return NotFound(new { success = false, mensaje = "Comuna no encontrada para la región indicada." });

        var actualizado = await _comunaRepositorio.ActualizarComunaAsync(idRegion, solicitud.IdComuna,
            solicitud.Comuna.Trim(), solicitud.Superficie, solicitud.Poblacion);

        if (!actualizado)
            return Conflict(new { success = false, mensaje = "No fue posible actualizar la comuna." });

        _logger.LogInformation("Comuna {IdComuna} de región {IdRegion} actualizada.", solicitud.IdComuna, idRegion);

        return Ok(new {success = true, mensaje = "Información de la comuna actualizada correctamente." });
      
    }

   
}


