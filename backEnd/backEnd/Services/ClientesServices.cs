using backEnd.Interfaces;
using backEnd.Models;
using backEnd.Models.Request;
using backEnd.Models.Response;
using System.Data;
using System.Linq;

namespace backEnd.Services
{
    public class ClientesServices: IClientes
    {
        public AltaClienteResponse AltaCliente(AltaClienteRequest input)
        {
            AltaClienteResponse altaClienteResponse = new AltaClienteResponse();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                try
                {

                    var clienteEditar = db.Clientes.FirstOrDefault(a => a.IdCliente == input.Cliente.IdCliente);

                    if (clienteEditar is not null && clienteEditar.IdCliente > 0)
                    {
                        clienteEditar.IdCliente = input.Cliente.IdCliente;
                        clienteEditar.Nombres = input.Cliente.Nombres;
                        clienteEditar.Apellidos = input.Cliente.Apellidos;
                        clienteEditar.Direccion = input.Cliente.Direccion;
                        db.SaveChanges();

                        altaClienteResponse.Exito = 1;
                        altaClienteResponse.Mensaje = "Se actualizó el cliente.";
                    }
                    else
                    {   
                        Cliente clienteNuevo = new Cliente();
                        var maxNumCliente = db.Clientes.Max(x => (int?)x.IdCliente) ?? 0;
                        maxNumCliente = maxNumCliente + 1;

                        clienteNuevo.IdCliente = maxNumCliente;
                        clienteNuevo.Nombres = input.Cliente.Nombres;
                        clienteNuevo.Apellidos = input.Cliente.Apellidos;
                        clienteNuevo.Direccion = input.Cliente.Direccion;
                        db.Clientes.Add(clienteNuevo);
                        db.SaveChanges();
                        altaClienteResponse.Exito = 1;
                        altaClienteResponse.Mensaje = "Cliente creado correctamente";

                    }
                }
                catch (DataException /* dex */)
                {
                    altaClienteResponse.Exito = 0;
                    altaClienteResponse.Mensaje = "No fue posible crear el cliente, intentar nuevamente.";
                }
            }

            return altaClienteResponse;
        }


        public RecuperarClientesResponse RecuperarClientes(RecuperarClientesRequest input)
        {
            RecuperarClientesResponse ListaClientesResponse = new RecuperarClientesResponse();

            using (loyaltyclubContext db = new loyaltyclubContext())
            {
                var lst = (from e in db.Clientes
                           where e.IdCliente != 0
                           select new Cliente
                           {
                               IdCliente = e.IdCliente,
                               Nombres = e.Nombres,
                               Apellidos = e.Apellidos,
                               Direccion = e.Direccion,
                               UserName = e.UserName,
                               Password = e.Password
                           }).ToList();

                ListaClientesResponse.ListaClientes = lst;
            }

            return ListaClientesResponse;
        }
    }
}
