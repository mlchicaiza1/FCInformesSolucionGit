using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Models
{
    public class MenuModel
    {
        public MenuModel()
        {
            MenuItems = new List<MenuModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ApplicationRole { get; set; }
        public int Order { get; set; }
        public int ParentId { get; set; }                        
        public List<MenuModel> MenuItems { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class MenuViewModel
    {   
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
                
        [Display(Name = "Url")]
        public string Url { get; set; }
        
        [Display(Name = "Rol de Aplicación")]
        public string ApplicationRole { get; set; }

        [Required]
        [Display(Name = "Órden")]
        public int Order { get; set; }

        [Display(Name = "Menú Superior")]        
        public int? ParentId { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class MenuAccessModel
    {
        public MenuAccessModel()
        {
            MenuItems = new List<MenuAccessModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

        public List<MenuAccessModel> MenuItems { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}