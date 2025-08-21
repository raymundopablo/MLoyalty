using backEnd.clases;

namespace backEnd.Models.Response
{
    public class RecuperarAspNetUserPorIdResponse: Respuesta
    {
        public cs_AspNetUser Usuario { get; set; }
    }
}
