using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCInformesSolucion.DAL.Entities
{
    [Table("Configurations")]
    public class Configuration : Entity
    {
        public int RequestNextNumber { get; set; }


        [MaxLength(2000)]
        public string RequestLegalScript { get; set; }

        [MaxLength(250)]
        public string FilesPath { get; set; }

        [MaxLength(250)]
        public string EmailFrom { get; set; }

        [MaxLength(250)]
        public string SmtpServer { get; set; }

        [MaxLength(250)]
        public string SmptUser { get; set; }

        [MaxLength(250)]
        public string SmptPassword { get; set; }        

        public bool SmptEnableSsl { get; set; }
                
        public int SmptPort { get; set; }

    }
}
