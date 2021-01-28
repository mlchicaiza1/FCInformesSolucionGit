using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCInformesSolucion.DAL.Entities
{
    [Table("Menus")]
    public class Menu : Entity
    {
        public Menu()
        {   
            MenuItems = new List<Menu>();
        }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        [MaxLength(250)]
        public string Url { get; set; }
        public int Order { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }        
        public virtual Menu Parent { get; set; }

        [MaxLength(36)]
        public string ApplicationRole { get; set; }

        [NotMapped] 
        public string DescripcionCompleta => $"{Name} ({Description})";

        [NotMapped]
        public List<Menu> MenuItems { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
