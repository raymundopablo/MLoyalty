using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            ClienteArticulos = new HashSet<ClienteArticulo>();
        }

        public int IdCliente { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ClienteArticulo> ClienteArticulos { get; set; }
    }
}
