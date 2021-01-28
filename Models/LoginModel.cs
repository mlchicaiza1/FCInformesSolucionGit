using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Usuario")]        
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        public string Password { get; set; }        
    }
}