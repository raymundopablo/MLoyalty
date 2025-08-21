using System.Collections.Generic;

namespace backEnd.Models.Response
{
    public class RecuperarArticulosPorTiendaResponse: Respuesta
    {
        public List<Articulo> ListaArticulos { get; set; }
    }
}
