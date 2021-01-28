using FCInformesSolucion.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public int? AgencyId { get; set; }
        public int RequestNumber { get; set; }
        public string UserName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Identification { get; set; }
        public string FullNames { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string Place { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
        public int? BankId { get; set; }
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string Attachment1Name { get; set; }
        public byte[] Attachment1 { get; set; }

        public string Attachment2Name { get; set; }
        public byte[] Attachment2 { get; set; }
        public string Attachment3Name { get; set; }
        public byte[] Attachment3 { get; set; }

        public string Attachment4Name { get; set; }
        public byte[] Attachment4 { get; set; }

        public int RequestStatus { get; set; }
    }

    public class RequestViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Número de Solicitud")]
        public int RequestNumber { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Fecha Requerimiento")]        
        public string RequestDate { get; set; }

        [Required]
        [IdentificationValid]
        [Display(Name = "CC/RUC")]
        public string Identification { get; set; }

        [Required]
        [Display(Name = "Nombre/Razón Social")]
        public string FullNames { get; set; }
                
        [Display(Name = "Provincia")]
        public string Province { get; set; }

        [Display(Name = "Provincia")]
        public int? ProvinceId { get; set; }
                
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Ciudad")]
        public int? CityId { get; set; }

        [Display(Name = "Cantón/Parroquia")]
        public string Place { get; set; }


        [Display(Name = "Teléfono Filjo")]
        public string Telephone { get; set; }


        [Display(Name = "Celular")]
        public string Cellphone { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Banco")]
        public string Bank { get; set; }

        [Display(Name = "Banco")]
        public int? BankId { get; set; }

        [Required]
        [Display(Name = "Requiriente")]
        public string ApplicantName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public string Attachment1Name { get; set; }

        public string Attachment2Name { get; set; }

        public string Attachment3Name { get; set; }

        public string Attachment4Name { get; set; }

        [Display(Name = "Estado Solicitud")]
        public int RequestStatus { get; set; }

        public int RequestProcessStatus { get; set; }

        [Required]
        [Display(Name = "Sucursal")]
        public string AgencyId { get; set; }

        [Display(Name = "Sucursal")]
        public string Agency { get; set; }
        public string RequestLegalScript { get; set; }

        [Display(Name = "Estado Actual")]
        public string ProcessStatus { get; set; }


        [Display(Name = "Estado Actual")]
        public int ProccesStatusId { get; set; }

        [Display(Name = "Cupo Inicial Sugerido")]
        public decimal? SuggestedInitialQuota { get; set; }
        public DateTime? ProcessedDate { get; set; }

        [Display(Name = "Observaciones")]
        public string ProcessRemarks { get; set; }

    }

    public class RequestSendByEmailViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Message { get; set; }        
    }

    public class RequestProcessViewModel
    {
        public int? Id { get; set; }
        public int ProccesStatusId { get; set; }
        public decimal? SuggestedInitialQuota { get; set; }
        public int RequestProcessStatus { get; set; }
        public string ProcessedDate { get; set; }
        public string ProcessRemarks { get; set; }
    }
}