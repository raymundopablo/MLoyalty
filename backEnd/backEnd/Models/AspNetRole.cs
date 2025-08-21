using System;
using System.Collections.Generic;

namespace backEnd.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            Users = new HashSet<AspNetUser>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
