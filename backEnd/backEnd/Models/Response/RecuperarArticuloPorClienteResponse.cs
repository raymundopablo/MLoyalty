using System.Collections.Generic;

namespace backEnd.Models.Response
{
    public class RecuperarArticuloPorClienteResponse: Respuesta
    {
        public List<Articulo> ListaArticulos { get; set; }
    }
}
