using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCInformesSolucion.DAL.Entities
{
    [Table("Requests")]
    public class Request : Entity
    {
        [ForeignKey("Agency")]
        public int? AgencyId { get; set; }
        public virtual Agency Agency { get; set; }

        public int RequestNumber { get; set; }
        public DateTime RequestDate { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(13)]
        public string Identification { get; set; }

        [MaxLength(250)]
        public string FullNames { get; set; }

        [ForeignKey("Province")]
        public int? ProvinceId { get; set; }
        public virtual Province Province { get; set; }

        [ForeignKey("City")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        [MaxLength(100)]
        public string Place { get; set; }

        [MaxLength(20)]
        public string Telephone { get; set; }

        [MaxLength(20)]
        public string Cellphone { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [ForeignKey("Bank")]
        public int? BankId { get; set; }
        public virtual Province Bank { get; set; }

        [MaxLength(250)]
        public string ApplicantName { get; set; }

        [MaxLength(250)]
        public string Email { get; set; }

        [MaxLength(250)]
        public string Attachment1Name { get; set; }

        [MaxLength(250)]
        public string Attachment2Name { get; set; }

        [MaxLength(250)]
        public string Attachment3Name { get; set; }

        [MaxLength(250)]
        public string Attachment4Name { get; set; }

        public RequestStatus RequestStatus { get; set; }
        public RequestStatus? RequestProcessStatus { get; set; }

        [ForeignKey("ProccesStatus")]
        public int? ProccesStatusId { get; set; }
        public virtual ProccesStatus ProccesStatus { get; set; }
                
        public decimal? SuggestedInitialQuota { get; set; }
        public DateTime? ProcessedDate { get; set; }
        
        [MaxLength(500)]
        public string ProcessRemarks { get; set; }

        [MaxLength(100)]
        public string ProcessUserName { get; set; }

        public override string ToString()
        {
            return FullNames;
        }
    }
}
