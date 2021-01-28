using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.DAL.Entities
{
    public class ApplicationUserMenu : Entity
    {
        public ApplicationUserMenu()
        {            
        }

        [MaxLength(36)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
    }
}