using backEnd.clases;
using backEnd.Models.Response;

namespace backEnd.Models.Request
{
    public class AuthResponse: Respuesta
    {
        public object Datos { get; set; }
    }
}
