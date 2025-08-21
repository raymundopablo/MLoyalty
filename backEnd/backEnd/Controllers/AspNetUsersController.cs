using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using backEnd.clases;
using backEnd.Models.Response;
using backEnd.Models.Request;
using System.Data;
using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using backEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        [HttpPost("insertarAspNetUser")]
        //[Authorize]
        public IActionResult Post([FromBody] cs_AspNetUser cs_aspNetUser)
        {
            InsertarAspNetUserResponse Respuesta = new();
            DBConnection DBConnection = new DBConnection();


            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                try
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        var registroEditar = db.AspNetUsers.FirstOrDefault(x => x.Id == cs_aspNetUser.Id);

                        if (registroEditar is not null)
                        {

                            registroEditar.Id = cs_aspNetUser.Id;
                            registroEditar.Email = cs_aspNetUser.Email;
                            if (registroEditar.Password != cs_aspNetUser.Password)
                            {
                                //db.Entry(registroEditar).Property(x => x.Password).IsModified = true;
                                registroEditar.Password = Encriptar.GetSHA256(cs_aspNetUser.Password);
                            }
                            else
                            {
                                registroEditar.Password = cs_aspNetUser.Password;
                            }
                            registroEditar.PhoneNumber = cs_aspNetUser.PhoneNumber;
                            registroEditar.UserName = cs_aspNetUser.UserName;
                            registroEditar.Nombre = cs_aspNetUser.Nombre;
                            registroEditar.Apellidos = cs_aspNetUser.Apellidos;
                            db.SaveChanges();

                            dbContextTransaction.Commit();

                                DataTable TablaMensaje = new DataTable();

                                List<SqlParameter> Parameters = new List<SqlParameter>() {
                                         new SqlParameter{ ParameterName="@Opcion", Value= "updateUsuarioRol"},
                                         new SqlParameter{ ParameterName="@Id", Value= cs_aspNetUser.Id},
                                         new SqlParameter{ ParameterName="@Rol", Value= cs_aspNetUser.Rol}
                                         };


                                TablaMensaje = DBConnection.ExecProcedure_getDataReader(Parameters, "DefaultConnection", "AspNetUsers_ABC", "");

                                foreach (DataRow row in TablaMensaje.Rows)
                                {
                                    Respuesta.Exito = Convert.ToInt32(row["Result"]);
                                    Respuesta.Mensaje = row["Msg"].ToString();
                                }
                            
                        }
                        else
                        {
                            try
                            {

                                DataTable TablaMensaje = new DataTable();

                                List<SqlParameter> Parameters = new List<SqlParameter>() {
                                         new SqlParameter{ ParameterName="@Opcion", Value= "insertarUsuario"},
                                         new SqlParameter{ ParameterName="@Id", Value= "" },
                                         new SqlParameter{ ParameterName="@Email", Value= cs_aspNetUser.Email},
                                         new SqlParameter{ ParameterName="@Password", Value=  Encriptar.GetSHA256(cs_aspNetUser.Password) },
                                         new SqlParameter{ ParameterName="@PhoneNumber", Value= cs_aspNetUser.PhoneNumber},
                                         new SqlParameter{ ParameterName="@UserName", Value= cs_aspNetUser.UserName},
                                         new SqlParameter{ ParameterName="@Nombre", Value= cs_aspNetUser.Nombre},
                                         new SqlParameter{ ParameterName="@Apellidos", Value= cs_aspNetUser.Apellidos},
                                         new SqlParameter{ ParameterName="@Rol", Value= cs_aspNetUser.Rol}
                                         };


                                TablaMensaje = DBConnection.ExecProcedure_getDataReader(Parameters, "DefaultConnection", "AspNetUsers_ABC", "");

                                foreach (DataRow row in TablaMensaje.Rows)
                                {
                                    Respuesta.Exito = Convert.ToInt32(row["Result"]);
                                    Respuesta.Mensaje = row["Msg"].ToString();
                                }



                            }

                            catch (DataException /* dex */)
                            {
                                Respuesta.Exito = 0;
                                Respuesta.Mensaje = "No fué posible generar la cancelación, intentar nuevamente.";
                            }


                        }

                        Respuesta.Exito = 1;
                        Respuesta.Mensaje = "Se guardó correctamente";
                        Respuesta.Usuario = cs_aspNetUser;

                    }

                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.

                    Respuesta.Exito = 0;
                    Respuesta.Mensaje = "No se pudo guardar el elemento en el índice, intentar nuevamente.";
                }
            }


            return new JsonResult(Respuesta);
        }

        // GET: api/<AvaluosController>
        [HttpGet("recuperarAspNetUsers")]
        //[Authorize(Roles = ("Administrador"))]
        //[Authorize]
        public IEnumerable<cs_AspNetUser> recuperarAspNetUsers()
        {
            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                var lst = (from user in db.AspNetUsers
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
                                   Apellidos = user.Apellidos,
                               }).ToList();

                return lst;

            }
        }

        [HttpGet("{Id}")]
        //[Authorize]
        public IActionResult Get(string Id)
        {
            RecuperarAspNetUserPorIdResponse Respuesta = new();

            cs_AspNetUser cs_AspNetUser = new cs_AspNetUser();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                var aspNetUser = (from user in db.AspNetUsers
                                  from role in user.Roles
                                  join roleInfo in db.AspNetRoles on role.Id equals roleInfo.Id
                                  where user.Id ==  Id
                                  select new cs_AspNetUser
                                  {
                                      Id = Id,
                                      Email = user.Email,
                                      Password = user.Password,
                                      PhoneNumber = user.PhoneNumber,
                                      Rol = db.AspNetRoles.FirstOrDefault(x => x.Id == role.Id).Id,
                                      RolName = db.AspNetRoles.FirstOrDefault(x => x.Id == role.Id).Name,
                                      UserName = user.UserName,
                                      Nombre = user.Nombre,
                                      Apellidos = user.Apellidos,
                                  }).FirstOrDefault();

                cs_AspNetUser.Id = aspNetUser.Id;
                cs_AspNetUser.Email = aspNetUser.Email;
                cs_AspNetUser.Password = aspNetUser.Password;
                cs_AspNetUser.PhoneNumber = aspNetUser.PhoneNumber;
                cs_AspNetUser.Rol = aspNetUser.Rol;
                cs_AspNetUser.UserName = aspNetUser.UserName;
                cs_AspNetUser.Nombre = aspNetUser.Nombre;
                cs_AspNetUser.Apellidos = aspNetUser.Apellidos;
                cs_AspNetUser.RolName = aspNetUser.RolName;


                if (Id == null)
                {
                    Respuesta.Exito = 0;
                    Respuesta.Mensaje = "No se encontró el usuario.";

                }
                else
                {
                    Respuesta.Exito = 1;
                    Respuesta.Mensaje = "Exito";
                    Respuesta.Usuario = cs_AspNetUser;
                }

            }
            return new JsonResult(Respuesta);
        }


        [HttpPost("eliminarUser/{Id}")]
        //[Authorize]
        public IActionResult Post(int id)
        {
            Respuesta Respuesta = new Respuesta();
            DBConnection DBConnection = new DBConnection();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                try
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        var registroEliminar = db.AspNetUsers.FirstOrDefault(x => x.Id == Convert.ToString(id));

                        if (registroEliminar is not null)
                        {

                            DataTable TablaMensaje = new DataTable();

                            List<SqlParameter> Parameters = new List<SqlParameter>() {
                                         new SqlParameter{ ParameterName="@Opcion", Value= "eliminarUsuario"},
                                         new SqlParameter{ ParameterName="@Id", Value= registroEliminar.Id },
                                         };


                            TablaMensaje = DBConnection.ExecProcedure_getDataReader(Parameters, "DefaultConnection", "AspNetUsers_ABC", "");

                            foreach (DataRow row in TablaMensaje.Rows)
                            {
                                Respuesta.Exito = Convert.ToInt32(row["Result"]);
                                Respuesta.Mensaje = row["Msg"].ToString();
                            }

                        }
                        else
                        {
                            Respuesta.Exito = 0;
                            Respuesta.Mensaje = "No fué posible eliminar el usuario.";

                        }



                        dbContextTransaction.Commit();
                    }

                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.

                    Respuesta.Exito = 0;
                    Respuesta.Mensaje = "No fue posible eliminar el usuario, intentar nuevamente.";
                }
            }


            return new JsonResult(Respuesta);
        }


    }
}
