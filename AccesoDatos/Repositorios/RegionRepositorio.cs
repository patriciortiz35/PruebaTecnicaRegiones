using System.Data;
using AccesoDatos.Interfaces;
using AccesoDatos.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccesoDatos.Repositorios;

public class RegionRepositorio(IConfiguration configuration) : IRegionRepositorio
{
    private readonly string _connectionString = configuration.GetConnectionString("ConexionBD")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión ConexionBD.");

    private IDbConnection CrearConexion => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Region>> ObtenerRegionesAsync()
    {
        using var conexion = CrearConexion;
        return await conexion.QueryAsync<Region>("usp_PVT_Region_ObtenerTodos", commandType: CommandType.StoredProcedure);
    }

    public async Task<Region?> ObtenerRegionAsync(int idRegion)
    {
        using var conexion = CrearConexion;
        return await conexion.QueryFirstOrDefaultAsync<Region>("usp_PVT_Region_ObtenerPorId",
            new { IdRegion = idRegion }, commandType: CommandType.StoredProcedure);
    }

}
