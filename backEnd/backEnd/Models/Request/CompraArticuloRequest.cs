namespace backEnd.Models.Request
{
    public class CompraArticuloRequest
    {
        public int IdTienda { get; set; }
        public int IdCliente { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
    }
}
