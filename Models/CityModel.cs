using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Models
{
    public class CityModel
    {
        public CityModel()
        {
           
        }
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public string Name { get; set; }        
        public override string ToString()
        {
            return Name;
        }
    }

    public class CityViewModel
    {   
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}