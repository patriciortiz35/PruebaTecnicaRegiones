using System.Xml.Linq;

namespace ApiPvtRegiones.Utils
{
    public class Utilitario
    {
        /// <summary>
        /// Mapea los datos XML de la comuna.
        /// Entrega un objeto anónimo (estilo json)
        /// con los datos de la comuna, incluyendo superficie, población y densidad.
        /// </summary>
        /// <param name="comuna">AccesoDatos.Modelos.Comuna</param>
        /// <returns>object</returns>
        public object MapearComuna(AccesoDatos.Modelos.Comuna comuna)
        {
            decimal superficie = 0;
            int poblacion = 0;

            try
            {
                if (!string.IsNullOrWhiteSpace(comuna.InformacionAdicional))
                {
                    var xml = XDocument.Parse(comuna.InformacionAdicional);

                    decimal.TryParse(
                        xml.Root?.Element("Superficie")?.Value,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out superficie);

                    int.TryParse(
                        xml.Root?.Element("Poblacion")?.Value,
                        out poblacion);
                }
            }
            catch (System.Xml.XmlException) { }

            decimal densidad = superficie > 0
                ? Math.Round((decimal)poblacion / superficie, 2)
                : 0m;

            return new
            {
                comuna.IdComuna,
                comuna.IdRegion,
                comuna.ComunaNombre,
                Superficie = superficie,
                Poblacion = poblacion,
                Densidad = densidad
            };
        }
    }
}
