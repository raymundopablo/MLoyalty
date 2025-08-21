using System.Collections.Generic;

namespace backEnd.Models.Response
{
    public class RecuperarClientesResponse: Respuesta
    {
        public List<Cliente> ListaClientes { get; set; }
    }
}
