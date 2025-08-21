using backEnd.clases;
using backEnd.Interfaces;
using backEnd.Models;
using backEnd.Models.Request;
using backEnd.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;

namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly IArticulo _articulos;

        public ArticulosController(IArticulo articulos)
        {
            _articulos = articulos;
        }


        [HttpPost("AltaArticulo")]
        public IActionResult AltaArticulo([FromForm] AltaArticuloRequest request)
        {

            AltaArticuloResponse articuloResponse = new();
            AltaArticuloRequest articuloRequest = new()
            {
                Articulo = request.Articulo,
                IdTienda = request.IdTienda,
                imagen = request.imagen
            };

            if (request is not null)
            {
                try
                {
                    articuloResponse = _articulos.AltaArticulo(request);
                }
                catch (DataException /* dex */)
                {
                    articuloResponse.Exito = 0;
                    articuloResponse.Mensaje = "No fue posible insertar el articulo, intentar nuevamente.";
                }

                return new JsonResult(articuloResponse);
            }
            else
            {
                articuloResponse.Exito = 0;
                articuloResponse.Mensaje = "Debes capturar los campos.";
            }

            return new JsonResult(articuloResponse);
        }




        [HttpGet("RecuperarArticulosPorCliente{IdCliente}")]
        public IActionResult RecuperarArticulosPorCliente(int IdCliente)
        {
            RecuperarArticuloPorClienteResponse articuloPorClienteResponse = new();
            RecuperarArticulosPorClienteRequest articulosPorClienteRequest = new() {
                IdCliente = IdCliente
            };
            try
            {

                articuloPorClienteResponse = _articulos.RecuperarArticulosPorCliente(articulosPorClienteRequest);
            }
            catch (DataException /* dex */)
            {
                articuloPorClienteResponse.Exito = 0;
                articuloPorClienteResponse.Mensaje = "No se pudieron reuperar los ciclos";
            }

            return new JsonResult(articuloPorClienteResponse);
        }



        [HttpGet("RecuperarArticulosPorTienda{IdTienda}")]
        public IActionResult RecuperarArticulosPorTienda(int IdTienda)
        {
            RecuperarArticulosPorTiendaResponse articuloPorTiendaResponse = new();
            RecuperarArticulosPorTiendaRequest articulosPorTiendaRequest = new()
            {
                IdTienda = IdTienda
            };
            try
            {

                articuloPorTiendaResponse = _articulos.RecuperarArticulosPorTienda(articulosPorTiendaRequest);
            }
            catch (DataException /* dex */)
            {
                articuloPorTiendaResponse.Exito = 0;
                articuloPorTiendaResponse.Mensaje = "No se pudieron reuperar los ciclos";
            }

            return new JsonResult(articuloPorTiendaResponse);
        }


        [HttpPost("CompraArticulo")]
        public IActionResult CompraArticulo([FromBody] cs_Compra compra)
        {

            CompraArticuloResposnse compraResponse = new();
            CompraArticuloRequest articuloRequest = new()
            {
                IdArticulo = compra.IdArticulo,
                IdTienda = compra.IdTienda,
                Cantidad = compra.Cantidad,
                IdCliente = compra.IdCliente
            };

            if (compra.IdArticulo > 0 && compra.IdTienda > 0 && compra.Cantidad > 0)
            {
                try
                {
                    compraResponse = _articulos.CompraArticulo(articuloRequest);
                }
                catch (DataException /* dex */)
                {
                    compraResponse.Exito = 0;
                    compraResponse.Mensaje = "No fue posible comprar el articulo, intentar nuevamente.";
                }

                return new JsonResult(compraResponse);
            }
            else
            {
                compraResponse.Exito = 0;
                compraResponse.Mensaje = "Debes capturar un artículo o cantidad.";
            }

            return new JsonResult(compraResponse);
        }


        [HttpPost("eliminarArticulo/{IdAticulo}/{IdTienda}")]
        public IActionResult eliminarArticulo(int IdAticulo, int IdTienda)
        {
            Respuesta Respuesta = new Respuesta();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                try
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        var registroEliminar = db.TiendaArticulos.FirstOrDefault(x => x.IdArticulo == IdAticulo && x.IdTienda == IdTienda);

                        if (registroEliminar is not null)
                        {
                            db.Entry(registroEliminar).State = EntityState.Deleted;
                            db.SaveChanges();
                            Respuesta.Exito = 1;
                            Respuesta.Mensaje = "Se eliminó correctamente el artículo de la tienda.";
                        }
                        else
                        {
                            Respuesta.Exito = 0;
                            Respuesta.Mensaje = "No fué posible eliminar el artículo.";
                        }
                        dbContextTransaction.Commit();
                    }

                }
                catch (DataException /* dex */)
                {
                    Respuesta.Exito = 0;
                    Respuesta.Mensaje = "No fué posible eliminar el artículo., intentar nuevamente.";
                }
            }


            return new JsonResult(Respuesta);
        }


        [HttpPost("eliminarArticuloCliente/{IdAticulo}/{IdCliente}")]
        public IActionResult eliminarArticuloCliente(int IdAticulo, int IdCliente)
        {
            Respuesta Respuesta = new Respuesta();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                try
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        var registroEliminar = db.ClienteArticulos.FirstOrDefault(x => x.IdArticulo == IdAticulo && x.IdCliente == IdCliente);

                        if (registroEliminar is not null)
                        {
                            db.Entry(registroEliminar).State = EntityState.Deleted;
                            db.SaveChanges();
                            Respuesta.Exito = 1;
                            Respuesta.Mensaje = "Se eliminó correctamente el artículo del cliente.";
                        }
                        else
                        {
                            Respuesta.Exito = 0;
                            Respuesta.Mensaje = "No fué posible eliminar el artículo.";
                        }
                        dbContextTransaction.Commit();
                    }

                }
                catch (DataException /* dex */)
                {
                    Respuesta.Exito = 0;
                    Respuesta.Mensaje = "No fué posible eliminar el artículo., intentar nuevamente.";
                }
            }


            return new JsonResult(Respuesta);
        }
    }
}
