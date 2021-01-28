using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Models
{
    public class AgencyModel
    {
        public AgencyModel()
        {
           
        }
        public int Id { get; set; }
        public string Name { get; set; }        
        public override string ToString()
        {
            return Name;
        }
    }

    public class AgencyViewModel
    {   
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}