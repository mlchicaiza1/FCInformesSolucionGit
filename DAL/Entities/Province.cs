using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCInformesSolucion.DAL.Entities
{
    [Table("Provinces")]
    public class Province : Entity
    {  

        [MaxLength(100)]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
