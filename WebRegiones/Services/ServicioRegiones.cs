using WebRegiones.Models;
namespace WebRegiones.Services;
public class ServicioRegiones(HttpClient httpClient, ILogger<ServicioRegiones> logger) : IServicioRegiones
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<ServicioRegiones> _logger = logger;

    public async Task<IEnumerable<RegionViewModel>> ObtenerRegionesAsync()
        => await _httpClient.GetFromJsonAsync<IEnumerable<RegionViewModel>>("api/region") ?? [];

    public async Task<IEnumerable<ComunaViewModel>> ObtenerComunasAsync(int idRegion)
        => await _httpClient.GetFromJsonAsync<IEnumerable<ComunaViewModel>>($"api/region/{idRegion}/comuna") ?? [];

    public async Task<ComunaViewModel?> ObtenerComunaAsync(int idRegion, int idComuna)
        => await _httpClient.GetFromJsonAsync<ComunaViewModel>($"api/region/{idRegion}/comuna/{idComuna}");

    public async Task ActualizarComunaAsync(ComunaViewModel modelo)
    {
        var respuesta = await _httpClient.PostAsJsonAsync($"api/region/{modelo.IdRegion}/comuna", new
        {
            modelo.IdComuna, Comuna = modelo.ComunaNombre, modelo.Superficie, modelo.Poblacion
        });
        if (!respuesta.IsSuccessStatusCode)
        {
            var detalle = await respuesta.Content.ReadAsStringAsync();
            _logger.LogWarning("La API rechazó la actualización de la comuna {IdComuna}: {Respuesta}", modelo.IdComuna, detalle);
            throw new HttpRequestException("No fue posible actualizar la información. " + detalle, null, respuesta.StatusCode);
        }
    }
}
