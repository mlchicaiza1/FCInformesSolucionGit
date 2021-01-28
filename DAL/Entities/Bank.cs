using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCInformesSolucion.DAL.Entities
{
    [Table("Banks")]
    public class Bank : Entity
    {  

        [MaxLength(100)]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
