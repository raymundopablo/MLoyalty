using backEnd.clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backEnd.Models.Request
{
    public class AltaArticuloRequest
    {
        [FromForm] public string Articulo { get; set; }
        [FromForm] public string IdTienda { get; set; }
        [FromForm] public IFormFile imagen { get; set; }
    }
}
