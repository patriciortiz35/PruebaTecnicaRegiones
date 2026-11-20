namespace AccesoDatos.Modelos;

public class Comuna
{
    public int IdComuna { get; set; }
    public int IdRegion { get; set; }
    public string ComunaNombre { get; set; } = string.Empty;
    public string? InformacionAdicional { get; set; }
}
