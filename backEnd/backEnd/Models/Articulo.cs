using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class Articulo
    {
        public Articulo()
        {
            ClienteArticulos = new HashSet<ClienteArticulo>();
            TiendaArticulos = new HashSet<TiendaArticulo>();
        }

        public int IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; }

        public virtual ICollection<ClienteArticulo> ClienteArticulos { get; set; }
        public virtual ICollection<TiendaArticulo> TiendaArticulos { get; set; }
    }
}
