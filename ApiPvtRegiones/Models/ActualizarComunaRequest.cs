using System.ComponentModel.DataAnnotations;

namespace ApiPvtRegiones.Models
{
    public class ActualizarComunaRequest
    {
        [Range(1, int.MaxValue), Required(ErrorMessage = "ID comuna es requerido")] 
        public int IdComuna { get; set; }
        
        [Required(ErrorMessage = "Nombre comuna es requerido"), StringLength(128, MinimumLength = 2)] 
        public string Comuna { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true)]
        public decimal Superficie { get; set; }
        
        [Range(0, int.MaxValue)] 
        public int Poblacion { get; set; }
    }
}
