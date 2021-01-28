using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCInformesSolucion.DAL.Entities
{
    [Table("Cities")]
    public class City : Entity
    {   

        [ForeignKey("Province")]
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
