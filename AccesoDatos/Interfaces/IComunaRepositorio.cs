using AccesoDatos.Modelos;

namespace AccesoDatos.Interfaces
{
    public interface IComunaRepositorio
    {
        /// <summary>
        /// Obtiene listado de comunas por ID región
        /// </summary>
        /// <param name="idRegion">int</param>
        /// <returns>IEnumerable<Comuna></returns>
        Task<IEnumerable<Comuna>> ObtenerComunasAsync(int idRegion);

        /// <summary>
        /// Obtiene comuna por su ID región y ID comuna.
        /// </summary>
        /// <param name="idRegion">int</param>
        /// <param name="idComuna">int</param>
        /// <returns>Comuna?</returns>
        Task<Comuna?> ObtenerComunaAsync(int idRegion, int idComuna);

        /// <summary>
        /// Actualiza el registro de comuna.
        /// Retorna verdadero actualización Ok, falso en caso 
        /// contrario.
        /// </summary>
        /// <param name="idRegion">int</param>
        /// <param name="idComuna">int</param>
        /// <param name="comuna">string</param>
        /// <param name="superficie">decimal</param>
        /// <param name="poblacion">int</param>
        /// <returns>bool</returns>
        Task<bool> ActualizarComunaAsync(int idRegion, int idComuna, string comuna, decimal superficie, int poblacion);
    }
}
