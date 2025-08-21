using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class Tienda
    {
        public Tienda()
        {
            TiendaArticulos = new HashSet<TiendaArticulo>();
        }

        public int IdTienda { get; set; }
        public string Sucursal { get; set; }
        public string Direccion { get; set; }

        public virtual ICollection<TiendaArticulo> TiendaArticulos { get; set; }
    }
}
