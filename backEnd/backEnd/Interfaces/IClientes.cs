using backEnd.Models.Request;
using backEnd.Models.Response;

namespace backEnd.Interfaces
{
    public interface IClientes
    {
        AltaClienteResponse AltaCliente(AltaClienteRequest input);

        RecuperarClientesResponse RecuperarClientes(RecuperarClientesRequest input);
    }
}
