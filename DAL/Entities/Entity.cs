using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.DAL.Entities
{
    public class Entity
    {
        public Entity()
        {
            CreatedDate = DateTime.Now;
            Status = EntityStatus.Active;
        }
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public EntityStatus Status { get; set; }
    }
}