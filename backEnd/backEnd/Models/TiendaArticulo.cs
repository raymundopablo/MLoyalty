using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class TiendaArticulo
    {
        public int IdTienda { get; set; }
        public int IdArticulo { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Articulo IdArticuloNavigation { get; set; }
        public virtual Tienda IdTiendaNavigation { get; set; }
    }
}
