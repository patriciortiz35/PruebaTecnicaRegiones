using AccesoDatos.Modelos;

namespace AccesoDatos.Interfaces;

public interface IRegionRepositorio
{
    /// <summary>
    /// Obtiene listado de regiones
    /// </summary>
    /// <returns>IEnumerable<Region></returns>
    Task<IEnumerable<Region>> ObtenerRegionesAsync();

    /// <summary>
    /// Obtiene region por su ID
    /// </summary>
    /// <param name="idRegion">int</param>
    /// <returns>Region?</returns>
    Task<Region?> ObtenerRegionAsync(int idRegion);
   
}
