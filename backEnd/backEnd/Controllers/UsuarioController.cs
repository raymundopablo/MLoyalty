using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backEnd.clases;
using backEnd.Models;
using backEnd.Models.Request;
using backEnd.Models.Response;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using backEnd.Interfaces;
using backEnd.Services;
using NuGet.Packaging.Signing;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult login([FromBody] AuthRequest model)
        {
            AuthResponse Respuesta = new AuthResponse();

            try
            {
                using (loyaltyclubContext db = new loyaltyclubContext())
                {
                    string password = Encriptar.GetSHA256(model.Password);

                    //Trae los datos del usuario y su roll

                    var usuario = (from user in db.AspNetUsers
                                   where user.UserName == model.Usuario && user.Password == password
                                   from role in user.Roles
                                   join roleInfo in db.AspNetRoles on role.Id equals roleInfo.Id
                                   select new cs_AspNetUser
                                   {
                                       Id = user.Id,
                                       Email = user.Email,
                                       Password = user.Password,
                                       PhoneNumber = user.PhoneNumber,
                                       Rol = roleInfo.Id,
                                       RolName = roleInfo.Name,
                                       UserName = user.UserName,
                                       Nombre = user.Nombre,
                                       Apellidos = user.Apellidos
                                   }).FirstOrDefault();


                    if (usuario == null)
                    {
                        Respuesta.Exito = 0;
                        Respuesta.Mensaje = "Usuario o password incorrecto, favor de verificarlo.";
                        Respuesta.Datos = "";
                    }
                    else
                    {
                        usuario.Password = "";
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
                Respuesta.Mensaje = ex.Message;
            }

            return new JsonResult(Respuesta);
        }

    }
}