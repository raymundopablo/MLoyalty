using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            Roles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
