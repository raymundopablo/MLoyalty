using backEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendasController : ControllerBase
    {

        [HttpGet("RecuperarTiendas")]
        public IActionResult RecuperarTiendas()
        {
            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                var lst = (from e in db.Tiendas
                           select new Tienda
                           {
                               IdTienda = e.IdTienda,
                               Direccion = e.Direccion,
                               Sucursal = e.Sucursal
                           }).ToList();

                return new JsonResult(lst);

            }
           
        }
    }
}
