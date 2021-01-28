using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Models
{
    public class UserModel
    {
        public virtual string Id { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreatedDate { get; set; }        
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Password { get; set; }
    }

    public class NewUserViewModel
    {
        [Required]
        [Display(Name = "Nombres Completos")]
        public string UserFullName { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public virtual string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public virtual string Password { get; set; }
    }

    public class EditUserViewModel
    {
        public virtual string Id { get; set; }

        [Required]
        [Display(Name = "Nombres Completos")]
        public string UserFullName { get; set; }

        [Required]
        [Display(Name = "Usuario")]        
        public virtual string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
                
        [Display(Name = "Contraseña")]
        public virtual string Password { get; set; }
    }

    public class UserAccessModel {

        [Display(Name = "Usuario")]
        public string ApplicationUserId { get; set; }
        public int[] MenuIds { get; set; }
    }
}