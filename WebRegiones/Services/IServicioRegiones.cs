using WebRegiones.Models;
namespace WebRegiones.Services;

/// <summary>
/// Interfaz de servicio para la gestión de regiones y comunas.
/// </summary>
public interface IServicioRegiones
{
    /// <summary>
    /// Obtiene la lista de regiones disponibles.
    /// </summary>
    /// <returns>IEnumerable<RegionViewModel></returns>
    Task<IEnumerable<RegionViewModel>> ObtenerRegionesAsync();

    /// <summary>
    /// Obtiene la lista de comunas para una región específica.
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <returns>IEnumerable<ComunaViewModel></returns>
    Task<IEnumerable<ComunaViewModel>> ObtenerComunasAsync(int idRegion);

    /// <summary>
    /// Obtiene comuna específica de una región.
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <param name="idComuna">int</param>
    /// <returns>ComunaViewModel?</returns>
    Task<ComunaViewModel?> ObtenerComunaAsync(int idRegion, int idComuna);

    /// <summary>
    /// Actualiza la información de una comuna específica.
    /// </summary>
    /// <param name="modelo">ComunaViewModel</param>
    /// <returns></returns>
    Task ActualizarComunaAsync(ComunaViewModel modelo);
}
