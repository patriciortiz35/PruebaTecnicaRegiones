using System.Data;
using AccesoDatos.Interfaces;
using AccesoDatos.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Repositorios;

public class ComunaRepositorio(IConfiguration configuration) : IComunaRepositorio
{
    private readonly string _connectionString = configuration.GetConnectionString("ConexionBD")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión ConexionBD.");

    private IDbConnection CrearConexion => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Comuna>> ObtenerComunasAsync(int idRegion)
    {
        using var conexion = CrearConexion;
        return await conexion.QueryAsync<Comuna>("usp_PVT_Comuna_ObtenerPorRegion",
            new { IdRegion = idRegion }, commandType: CommandType.StoredProcedure);
    }

    public async Task<Comuna?> ObtenerComunaAsync(int idRegion, int idComuna)
    {
        using var conexion = CrearConexion;
        return await conexion.QueryFirstOrDefaultAsync<Comuna>("usp_PVT_Comuna_ObtenerPorId",
            new { IdRegion = idRegion, IdComuna = idComuna }, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ActualizarComunaAsync(int idRegion, int idComuna, string comuna, decimal superficie, int poblacion)
    {
        using var conexion = CrearConexion;
        var resultado = await conexion.QuerySingleAsync<int>("usp_PVT_Comuna_Actualizar",
            new { IdRegion = idRegion, IdComuna = idComuna, Comuna = comuna, Superficie = superficie, Poblacion = poblacion },
            commandType: CommandType.StoredProcedure);
        return resultado == 1;
    }
}
