using backEnd.clases;
using backEnd.Interfaces;
using backEnd.Models;
using backEnd.Models.Request;
using backEnd.Models.Response;
using backEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;

namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private readonly IClientes _clientes;

        public ClientesController(IClientes clientes)
        {
            _clientes = clientes;
        }

        [HttpGet("RecuperarClientes")]
        public IActionResult RecuperarClientes()
        {
            RecuperarClientesResponse recuperarClientesResponse = new();
            RecuperarClientesRequest recuperarClientesRequest = new();

            try
            {

                recuperarClientesResponse = _clientes.RecuperarClientes(recuperarClientesRequest);
            }
            catch (DataException /* dex */)
            {
                recuperarClientesResponse.Exito = 0;
                recuperarClientesResponse.Mensaje = "No se pudieron recuperar los clientes.";
            }

            return new JsonResult(recuperarClientesResponse);
        }


        [HttpPost("insertarCliente")]
        public IActionResult Post([FromBody] Cliente cliente)
        {

            AltaClienteResponse clienteResponse = new();
            AltaClienteRequest clienteRequest = new() { Cliente = cliente };

            if (cliente is not null)
            {
                try
                {
                    clienteResponse = _clientes.AltaCliente(clienteRequest);
                }
                catch (DataException /* dex */)
                {
                    clienteResponse.Exito = 0;
                    clienteResponse.Mensaje = "No fue posible insertar el cliente, intentar nuevamente.";
                }

                return new JsonResult(clienteResponse);
            }
            else
            {
                clienteResponse.Exito = 0;
                clienteResponse.Mensaje = "Debes capturar los campos.";
            }

            return new JsonResult(clienteResponse);
        }


        [HttpPost("eliminarCliente/{Id}")]
        public IActionResult Post(int id)
        {
            Respuesta Respuesta = new Respuesta();
                
            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                try
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        var registroEliminar = db.Clientes.FirstOrDefault(x => x.IdCliente == id);  
     
                        if (registroEliminar is not null)
                        {
                            db.Entry(registroEliminar).State = EntityState.Deleted;
                            db.SaveChanges();
                            Respuesta.Exito = 1;
                            Respuesta.Mensaje = "Se eliminó correctamente el cliente.";
                        }
                        else
                        {
                            Respuesta.Exito = 0;
                            Respuesta.Mensaje = "No fué posible eliminar el cliente.";
                        }
                        dbContextTransaction.Commit();
                    }

                }
                catch (DataException /* dex */)
                {
                    Respuesta.Exito = 0;
                    Respuesta.Mensaje = "No fue posible eliminar el cliente, intentar nuevamente.";
                }
            }


            return new JsonResult(Respuesta);
        }


        [HttpPost("login")]
        public IActionResult login([FromBody] AuthRequest model)
        {
            AuthResponse Respuesta = new AuthResponse();
            int IdEmpresa = 0;

            try
            {
                using (loyaltyclubContext db = new loyaltyclubContext())
                {
                    string password = Encriptar.GetSHA256(model.Password);

                    var usuario = (from user in db.Clientes
                                   where user.UserName == model.Usuario && user.Password == password
                                   select new cs_Cliente
                                   {
                                       IdCliente = user.IdCliente,
                                       Apellidos = user.Apellidos,
                                       Direccion = user.Direccion,
                                       UserName = user.UserName,
                                       Nombres = user.Nombres,
                                       RolName = "Cliente",
                                   }).FirstOrDefault();


                    if (usuario == null)
                    {
                        Respuesta.Exito = 0;
                        Respuesta.Mensaje = "Usuario o password incorrecto, favor de verificarlo.";
                    }
                    else
                    {
                        Respuesta.Exito = 1;
                        Respuesta.Datos = usuario;
                        Respuesta.Mensaje = "Usuario o password correcto";
                        return Ok(Respuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                Respuesta.Exito = 0;
                Respuesta.Mensaje = "Usuario o password incorrecto, favor de verificarlo.";
            }

            return new JsonResult(Respuesta);
        }

    }
}
