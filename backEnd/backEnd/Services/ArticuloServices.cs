using backEnd.clases;
using backEnd.Helpers;
using backEnd.Interfaces;
using backEnd.Models;
using backEnd.Models.Request;
using backEnd.Models.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace backEnd.Services
{

    public class ArticuloServices : IArticulo
    {
        private readonly IConfiguration _config;
        private readonly string filepath;


        public ArticuloServices(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            var rutaRelativa = config["Rutas:RutaImagenes"];
            filepath = Path.Combine(env.ContentRootPath, rutaRelativa);
        }
        public AltaArticuloResponse AltaArticulo(AltaArticuloRequest input)
        {
            AltaArticuloResponse Response = new();
            Respuesta respuesta = new Respuesta();
            cs_Articulo articuloObj = JsonConvert.DeserializeObject<cs_Articulo>(input.Articulo); 

            try
            {
                using (loyaltyclubContext db = new loyaltyclubContext())
                {
                    var esxiteArticuloEnTienda = db.TiendaArticulos.Where(x => x.IdArticulo == articuloObj.IdArticulo && x.IdTienda == Convert.ToInt32(input.IdTienda)).FirstOrDefault();
                    var articuloExistente = db.Articulos.Where(x => x.IdArticulo == articuloObj.IdArticulo).FirstOrDefault();

                    if (articuloExistente != null && esxiteArticuloEnTienda != null)
                    {
                        articuloExistente.Descripcion = articuloObj.Descripcion;
                        articuloExistente.Stock += articuloObj.Stock;
                        articuloExistente.Precio = articuloObj.Precio;
                        articuloExistente.Imagen = articuloObj.Imagen;
                        db.SaveChanges();
                    }
                    else
                    {
                        Articulo articulo = new()
                        {
                            Descripcion = articuloObj.Descripcion,
                            Stock = articuloObj.Stock,
                            Precio = articuloObj.Precio,
                            Imagen = input.imagen.FileName
                        };
                        db.Articulos.Add(articulo);
                        db.SaveChanges();

                        if (Convert.ToInt32(input.IdTienda) > 0)
                        {
                            TiendaArticulo ta = new()
                            {
                                IdTienda = Convert.ToInt32(input.IdTienda),
                                IdArticulo = articulo.IdArticulo,
                                Fecha = DateTime.Today
                            };

                            db.TiendaArticulos.Add(ta);
                            db.SaveChanges();
                        }
                        Response.Exito = 1;
                        Response.Mensaje = "Artículo dado de alta";

                        respuesta = GuardarArchivo(input.imagen);
                    }


                }
            }
            catch (DataException ex)
            {
                Response.Exito = 0;
                Response.Mensaje = "Error: " + ex.Message;
                return Response;
            }

            using (loyaltyclubContext db = new loyaltyclubContext())
            {

            }

            return Response;
        }



        Respuesta GuardarArchivo(IFormFile imagen)
        {
            Respuesta respuesta = new Respuesta();

            if (imagen.Length > 0)
            {
                try
                {
                    if (imagen.ContentType == "image/jpeg" || imagen.ContentType == "image/jpg" || imagen.ContentType == "image/png")
                    {
                        string NombreArchivo = imagen.FileName;

                        FileHelper fileHelper = new FileHelper();
                        try
                        {
                            respuesta.Exito = 1;
                            respuesta.Mensaje = fileHelper.SubirArchivo(filepath, "", NombreArchivo, imagen);
                        }
                        catch (DataException)
                        {
                            respuesta.Exito = 0;
                            respuesta.Mensaje = string.Empty;
                        }
                    }
                    else
                    {

                        respuesta.Exito = 0;
                        respuesta.Mensaje = string.Empty;

                    }
                }
                catch (DataException /* dex */)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = string.Empty;
                }
            }

            return respuesta;

        }

        public RecuperarArticuloPorClienteResponse RecuperarArticulosPorCliente(RecuperarArticulosPorClienteRequest input)
        {
            RecuperarArticuloPorClienteResponse Response = new();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                var lst = (from ca in db.ClienteArticulos
                           join a in db.Articulos on ca.IdArticulo equals a.IdArticulo
                           where ca.IdCliente == input.IdCliente
                           select a).ToList();

                Response.ListaArticulos = lst;
                Response.Exito = 1;
                Response.Mensaje = "Artículos recuperados";
            }

            return Response;
        }

        public RecuperarArticulosPorTiendaResponse RecuperarArticulosPorTienda(RecuperarArticulosPorTiendaRequest input)
        {
            RecuperarArticulosPorTiendaResponse Response = new();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                var lst = (from ta in db.TiendaArticulos
                           join a in db.Articulos on ta.IdArticulo equals a.IdArticulo
                           where ta.IdTienda == input.IdTienda
                           select a).ToList();

                Response.ListaArticulos = lst;
                Response.Exito = 1;
                Response.Mensaje = "Artículos recuperados";
            }

            return Response;
        }

        public AltaArticuloClienteResponse AltaArticuloCliente(AltaArticuloClienteRequest input)
        {
            AltaArticuloClienteResponse Response = new();

            try
            {
                using (loyaltyclubContext db = new loyaltyclubContext())
                {
                        ClienteArticulo clienteArticulo = new()
                        {
                            IdCliente = input.idCliente,
                            IdArticulo = input.idArticulo,
                            Fecha = DateTime.Today
                        };
                        db.ClienteArticulos.Add(clienteArticulo);
                        db.SaveChanges();
                        Response.Exito = 1;
                        Response.Mensaje = "Artículo agregado al cliente";
                }
                
            }
            catch (DataException ex)
            {
                Response.Exito = 0;
                Response.Mensaje = "Error: " + ex.Message;
                return Response;
            }

            return Response;
        }

        public CompraArticuloResposnse CompraArticulo(CompraArticuloRequest input)
        {
            CompraArticuloResposnse Response = new();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {

                var esxiteArticuloEnTienda = db.TiendaArticulos.Where(x => x.IdArticulo == input.IdArticulo && x.IdTienda == input.IdTienda).FirstOrDefault();
                var articuloExistente = db.Articulos.FirstOrDefault(x => x.IdArticulo == input.IdArticulo);

                if (articuloExistente != null && esxiteArticuloEnTienda != null)
                {
                    var clienteArticuloExistente = db.ClienteArticulos.FirstOrDefault(x => x.IdCliente == input.IdCliente && x.IdArticulo == input.IdArticulo);

                    if (articuloExistente.Stock < input.Cantidad)
                    {
                        Response.Exito = 0;
                        Response.Mensaje = "No hay suficiente stock del artículo";
                        return Response;
                    }

                    if (clienteArticuloExistente != null)
                    {
                        Response.Exito = 0;
                        Response.Mensaje = "El artículo ha sido comprado anteriormente por el cliente";
                        return Response;
                    }
                    else {
                        articuloExistente.Stock -= input.Cantidad;
                        db.SaveChanges();

                        AltaArticuloClienteResponse responseAltaArticuloCliente = new();
                        responseAltaArticuloCliente = AltaArticuloCliente(new AltaArticuloClienteRequest
                        {
                            idCliente = input.IdCliente,
                            idArticulo = input.IdArticulo
                        });

                        Response.Exito = responseAltaArticuloCliente.Exito;
                        Response.Mensaje = responseAltaArticuloCliente.Mensaje;
                    }
                }
                else
                {
                    Response.Exito = 0;
                    Response.Mensaje = "El artículo no existe";
                    return Response;
                }
            }

            return Response;
        }
    }
}
