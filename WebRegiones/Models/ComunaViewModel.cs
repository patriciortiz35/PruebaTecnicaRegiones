using System.ComponentModel.DataAnnotations;
namespace WebRegiones.Models;
public class ComunaViewModel
{
    public int IdComuna { get; set; }
    public int IdRegion { get; set; }
    
    [Required(ErrorMessage = "Ingrese el nombre de la comuna.")]
    [StringLength(128, MinimumLength = 2)]
    public string ComunaNombre { get; set; } = string.Empty;
    
    [Range(0.01, 999999999, ErrorMessage = "La superficie debe ser mayor a cero.")]
    public decimal Superficie { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "La población no puede ser negativa.")]
    public int Poblacion { get; set; }

    public decimal Densidad { get; set; }
}
