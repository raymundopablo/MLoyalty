using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backEnd.Models.Request
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Password { get; set; }
    }
}
