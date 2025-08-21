using backEnd.Models.Request;
using backEnd.Models.Response;

namespace backEnd.Interfaces
{
    public interface IArticulo
    {
        AltaArticuloResponse AltaArticulo(AltaArticuloRequest input);
        RecuperarArticulosPorTiendaResponse RecuperarArticulosPorTienda(RecuperarArticulosPorTiendaRequest input);
        RecuperarArticuloPorClienteResponse RecuperarArticulosPorCliente(RecuperarArticulosPorClienteRequest input);
        AltaArticuloClienteResponse AltaArticuloCliente(AltaArticuloClienteRequest input);
        CompraArticuloResposnse CompraArticulo(CompraArticuloRequest input);
    }
}
