using AutoMapper;
using FCInformesSolucion.Constants;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Helpers;
using FCInformesSolucion.Models;
using FCInformesSolucion.Services;
using Microsoft.AspNet.Identity;
using MvcJqGrid;
using MvcJqGrid.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FCInformesSolucion.Controllers
{
    [Authorize(Roles = "request,request_process")]
    public class RequestController : BaseMaintenanceController<Request, RequestModel, RequestViewModel>
    {
        IRequestService RequestService;
        IProvinceService ProvinceService;
        ICityService CityService;
        IBankService BankService;
        IConfigurationService ConfigurationService;
        IEmailService EmailService;
        IAccountService AccountService;
        IAgencyService AgencyService;
        IProccesStatusService ProccesStatusService;

        public bool Process { 
            get {
                return Request.QueryString["a"] == "Process";
            } 
        }

        public RequestController(
            IMapper mapper,
            IRequestService requestService,
            IProvinceService provinceService,
            ICityService cityService,
            IBankService bankService,
            IConfigurationService configurationService,
            IEmailService emailService,
            IAccountService accountService,
            IAgencyService agencyService,
            IProccesStatusService proccesStatusService)
            :base(mapper)
        {
            MaintenanceService =
            RequestService = requestService;
            ProvinceService = provinceService;
            CityService = cityService;
            BankService = bankService;
            ConfigurationService = configurationService;
            EmailService = emailService;
            AccountService = accountService;
            AgencyService = agencyService;
            ProccesStatusService = proccesStatusService;
            
        }
        protected override void OnIndex()
        {
            var users = AccountService
                            .AsQueryable()
                            .Select(u => new UserModel
                            {
                                Id = u.Id,
                                UserName = u.UserName
                            })
                            .ToList();
            users.Insert(0, new UserModel { UserName = "" });
            ViewBag.Users = users;

            var agencies = AgencyService
                            .AsQueryable()
                            .Select(u => new AgencyModel
                            {
                                Id = u.Id,
                                Name = u.Name
                            })
                            .ToList();
            agencies.Insert(0, new AgencyModel { Name = "" });
            ViewBag.Agencies = agencies;

            ViewBag.Process = Process;

        }

        protected override IQueryable<Request> ApplyFilters(IQueryable<Request> generalQuery, MvcJqGrid.Filter filter)
        {
            if (filter.rules?.Any() ?? false)
            {
                foreach (var rule in filter?.rules)
                {
                    var value = rule.data.ToLower().Trim();

                    if (rule.field == "search_query")
                    {                        
                        generalQuery =
                                generalQuery.Where(q => 
                                    q.RequestNumber.ToString().ToLower().Contains(value)
                                    || q.Agency.Name.ToLower().Contains(value)
                                    || q.Identification.ToLower().Contains(value)
                                    || q.FullNames.ToLower().Contains(value)
                                    || q.ApplicantName.ToLower().Contains(value)
                                );
                    }
                    else if (rule.field == "Identification")
                    {   
                        generalQuery =
                                generalQuery.Where(q =>                                    
                                        q.Identification.ToLower().Contains(value)
                                );
                    }
                    else if (rule.field == "FullNames")
                    {
                        generalQuery =
                                generalQuery.Where(q =>
                                        q.FullNames.ToLower().Contains(value)
                                );
                    }
                    else if (rule.field == "RequestDateFrom")
                    {
                        var from = Convert.ToDateTime(value);
                        generalQuery =
                                generalQuery.Where(q =>
                                        q.RequestDate >= from
                                );
                    }
                    else if (rule.field == "RequestDateTo")
                    {
                        var to = Convert.ToDateTime(value)
                                        .AddDays(1)
                                        .AddMilliseconds(-1);
                        generalQuery =
                                generalQuery.Where(q =>
                                        q.RequestDate <= to
                                );
                    }
                    else if (rule.field == "UserName")
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            generalQuery =
                                generalQuery.Where(q =>
                                        q.UserName == value
                                );
                        }                        
                    }
                    else if (rule.field == "ApplicantName")
                    {
                        generalQuery =
                                generalQuery.Where(q =>
                                        q.ApplicantName.ToLower().Contains(value)
                                );
                    }
                    else if (rule.field == "AgencyId")
                    {
                        var agencyId = Convert.ToInt32(value);
                        if (agencyId > 0)
                        {
                            generalQuery =
                                generalQuery.Where(q =>
                                        q.AgencyId == agencyId
                                );
                        }
                    }
                    else if (rule.field == "Identification")
                    {
                        generalQuery =
                                generalQuery.Where(q =>
                                        q.Identification.ToLower().Contains(value)
                                );
                    }
                }
            }
            return generalQuery;
        }

        protected override string[] GetRow(Request entity)
        {
            var actions = GridHelperExts.ActionsList("")
               .Add(GetViewAction(entity.Id))               
           .End();

            var viewUrl = !Process 
                ? Url.Action("View", new { id = entity.Id })
                : Url.Action("View", new { id = entity.Id, a = "Process" });

            return new[] {
                entity.Agency?.Name,
                $"<a href='{viewUrl}' class='btn-link-detalle'>{entity.RequestNumber.ToString("000000000")}</a>",
                entity.RequestDate.ToString(ApplicationContext.DateFormat),
                entity.Identification,
                entity.FullNames,
                entity.Province?.Name,
                entity.City?.Name,
                entity.RequestStatus == RequestStatus.Saved ? "Guardado" :
                entity.RequestStatus == RequestStatus.Validated ? "Validada":
                entity.RequestStatus == RequestStatus.Anulmment ? "Anulada" : ""
            };
        }

        public ActionResult GetClient(string identification)
        {

            var client = RequestService.AsQueryable()
                            .FirstOrDefault(c => c.Identification == identification
                                    && c.Status == EntityStatus.Active);

            if (client != null)
            {
                return Json(new
                {
                    success = true,
                    data = new { 
                        client.Identification,
                        client.FullNames,
                        client.ProvinceId,
                        client.CityId,
                        client.Place,
                        client.Telephone,
                        client.Cellphone,
                        client.Address,
                        client.BankId,
                        client.ApplicantName,
                        client.Email
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false,
                message = $"No se encontró cliente con identificación {identification}"
            }, JsonRequestBehavior.AllowGet);
        }

        protected override void OnNew(RequestViewModel requestModel)
        {
            requestModel.UserName = User.Identity.Name;            
            requestModel.RequestDate = DateTime.Now.ToString(ApplicationContext.DateTimeFormat);

            Init(requestModel);
        }

        //Guardar pdf, pasar de ViewModel a model
        protected override void OnNewPost(RequestViewModel viewModel, RequestModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    var buffer = new byte[file.ContentLength];
                    file.InputStream.Read(buffer, 0, buffer.Length);
                    model.Attachment1 = buffer;
                    model.Attachment1Name = file.FileName;

                    HttpPostedFileBase file1 = Request.Files[1];
                    var buffer1 = new byte[file1.ContentLength];
                    file1.InputStream.Read(buffer1, 0, buffer1.Length);
                    model.Attachment2 = buffer1;
                    model.Attachment2Name = file1.FileName;

                    HttpPostedFileBase file2 = Request.Files[2];
                    var buffer2 = new byte[file2.ContentLength];
                    file2.InputStream.Read(buffer2, 0, buffer2.Length);
                    model.Attachment3 = buffer2;
                    model.Attachment3Name = file2.FileName;

                    HttpPostedFileBase file3 = Request.Files[3];
                    var buffer3 = new byte[file3.ContentLength];
                    file3.InputStream.Read(buffer3, 0, buffer3.Length);
                    model.Attachment4 = buffer3;
                    model.Attachment4Name = file3.FileName;
                }
            }
            Init(viewModel);
        }

        protected override ActionResult ViewRedirectOnNew(Request request)
        {
            return RedirectToAction("View", new { request.Id });
        }

        protected override ActionResult ViewRedirectOnEdit(RequestViewModel viewModel, RequestModel model)
        {
            return RedirectToAction("View", new { model.Id });
        }

        public ActionResult View(int id)
        {
            var entity = MaintenanceService.AsQueryable().FirstOrDefault(u => u.Id == id);
            if (entity != null)
            {
                var viewModel = Mapper.Map<RequestViewModel>(entity);
                Init(viewModel);

                viewModel.ProcessStatus = entity.ProccesStatus?.Name;
                return View(viewModel);
            }
            return new HttpNotFoundResult();
        }

        protected override void OnEdit(RequestViewModel requestModel)
        {
            Init(requestModel);

            base.OnEdit(requestModel); 
        }

        protected override void OnEditPost(RequestViewModel viewModel, RequestModel model)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                var buffer = new byte[file.ContentLength];
                file.InputStream.Read(buffer, 0, buffer.Length);
                model.Attachment1 = buffer;
                model.Attachment1Name = file.FileName;

                HttpPostedFileBase file1 = Request.Files[1];
                var buffer1 = new byte[file1.ContentLength];
                file1.InputStream.Read(buffer1, 0, buffer1.Length);
                model.Attachment2 = buffer1;
                model.Attachment2Name = file1.FileName;

                HttpPostedFileBase file2 = Request.Files[2];
                var buffer2 = new byte[file2.ContentLength];
                file2.InputStream.Read(buffer2, 0, buffer2.Length);
                model.Attachment3 = buffer2;
                model.Attachment3Name = file2.FileName;

                HttpPostedFileBase file3 = Request.Files[3];
                var buffer3 = new byte[file3.ContentLength];
                file3.InputStream.Read(buffer3, 0, buffer3.Length);
                model.Attachment4 = buffer3;
                model.Attachment4Name = file3.FileName;
            }
        }

        public async Task <ActionResult> Attachment(int id, int index)
        {
            var attachmentUrl = await RequestService.GetAtachmentPathAsync(id, index);
            if (System.IO.File.Exists(attachmentUrl))
            {
                var mimeType = MimeMapping.GetMimeMapping(attachmentUrl);
                return File(attachmentUrl, mimeType);
            }
            return Content("");
        }

        public ActionResult Validate(int id)
        {
            try
            {
                RequestService.ValidateAsync(id);
                return Json(new
                {
                    success = true,
                    message = $"OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {   
                return Json(new
                {
                    success = false,
                    message = $"Error al validar solicitud. {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult Annulment(int id)
        {
            try
            {
                RequestService.AnulmmentAsync(id);
                return Json(new
                {
                    success = true,
                    message = $"OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error al anular solicitud. {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ProcessRequest(RequestProcessViewModel requestModel)
        {
            try
            {
                RequestService.ProcessAsync(requestModel);
                return Json(new
                {
                    success = true,
                    message = $"OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error al procesar solicitud. {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProcessValidate(int id)
        {
            try
            {
                RequestService.ValidateProcessAsync(id);
                return Json(new
                {
                    success = true,
                    message = $"OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error al validar solicitud. {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        public async Task<ActionResult> Print(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            var entity = await MaintenanceService.AsQueryable()
                            .FirstOrDefaultAsync(u => u.Id == id);
            if (entity != null)
            {
                var viewModel = Mapper.Map<RequestViewModel>(entity);

                var configuration = ConfigurationService.GetConfiguration();

                viewModel.Province = entity.Province?.Name;
                viewModel.City = entity.City?.Name;
                viewModel.Bank = entity.Bank?.Name;
                viewModel.Agency = entity.Agency?.Name;

                viewModel.RequestLegalScript = configuration.RequestLegalScript ?? "";
                viewModel.RequestLegalScript = viewModel.RequestLegalScript?.Replace("{FullNames}", viewModel.FullNames);
                viewModel.RequestLegalScript = viewModel.RequestLegalScript?.Replace("{ApplicantName}", viewModel.ApplicantName);

                return View(viewModel);
            }
            return Content("");
        }

        [AllowAnonymous]
        public ActionResult ExportPdf(int? id)
        {
            var x = new Rotativa.ActionAsPdf("Print", new { id });
            return x;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Print2(int? id)
        {
            if (id == null || id <= 0)
            {
                return HttpNotFound();
            }

            var entity = await MaintenanceService.AsQueryable()
                            .FirstOrDefaultAsync(u => u.Id == id);
            if (entity != null)
            {
                var viewModel = Mapper.Map<RequestViewModel>(entity);
                viewModel.ProcessStatus = entity.ProccesStatus?.Name;
                return View(viewModel);
            }
            return Content("");
        }

        [AllowAnonymous]
        public ActionResult ExportPdf2(int? id)
        {
            var x = new Rotativa.ActionAsPdf("Print2", new { id });
            return x;
        }

        public ActionResult SendEmail(RequestSendByEmailViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                return HttpNotFound();
            }

            try
            {
                var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri)
                {
                    Path = Url.Action("ExportPdf", "Request", new { id = model.Id }),
                    Query = null,
                };

                var url = urlBuilder.Uri.ToString();                
                var fileName = $"C:\\Temp\\{DateTime.Now.Ticks}_{model.Id}.pdf";                
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, fileName);
                }
                var attachments = new List<string> { fileName };
                var configuration = ConfigurationService.GetConfiguration();
                
                EmailService.SendEmail(
                    configuration.EmailFrom,
                    model.To,
                    model.Subject,
                    model.Message,
                    configuration.SmtpServer,
                    configuration.SmptUser,
                    configuration.SmptPassword,
                    configuration.SmptPort,
                    configuration.SmptEnableSsl,
                    attachments,
                    model.Cc
                );
                return Json(new { 
                    success = true
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult SendEmail2(RequestSendByEmailViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                return HttpNotFound();
            }

            try
            {
                var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri)
                {
                    Path = Url.Action("ExportPdf2", "Request", new { id = model.Id }),
                    Query = null,
                };

                var url = urlBuilder.Uri.ToString();
                var fileName = $"C:\\Temp\\{DateTime.Now.Ticks}_{model.Id}.pdf";
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(url, fileName);
                }
                var attachments = new List<string> { fileName };
                var configuration = ConfigurationService.GetConfiguration();

                EmailService.SendEmail(
                    configuration.EmailFrom,
                    model.To,
                    model.Subject,
                    model.Message,
                    configuration.SmtpServer,
                    configuration.SmptUser,
                    configuration.SmptPassword,
                    configuration.SmptPort,
                    configuration.SmptEnableSsl,
                    attachments,
                    model.Cc
                );
                return Json(new
                {
                    success = true
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.DenyGet);
            }
        }

        private void Init(RequestViewModel requestModel) {

            var provinces = ProvinceService.AsQueryable()
                                .Where(p => p.Status == EntityStatus.Active)
                                .Select(p => new ProvinceModel { 
                                    Id= p.Id,
                                    Name = p.Name
                                })
                                .ToList();

            ViewBag.Provinces = 
                new SelectList(provinces, "Id", "Name", requestModel.ProvinceId);

            if (requestModel.ProvinceId > 0)
            {

                var cities = CityService.AsQueryable()
                                .Where(c => c.ProvinceId == requestModel.ProvinceId
                                    && c.Status == EntityStatus.Active)
                                .Select(p => new CityModel
                                {
                                    Id = p.Id,
                                    Name = p.Name
                                })
                                .ToList();
                ViewBag.Cities =
                    new SelectList(cities, "Id", "Name", requestModel.CityId);

            }
            else
            {
                ViewBag.Cities =
                    new SelectList(new List<CityModel>(), "Id", "Name", requestModel.CityId);
            }

            var banks = BankService.AsQueryable()
                                .Where(p => p.Status == EntityStatus.Active)
                                .Select(p => new BankModel
                                {
                                    Id = p.Id,
                                    Name = p.Name
                                })
                                .ToList();

            ViewBag.Banks =
                new SelectList(banks, "Id", "Name", requestModel.BankId);


            var agencies = AgencyService.AsQueryable()
                                .Where(p => p.Status == EntityStatus.Active)
                                .Select(p => new AgencyModel
                                {
                                    Id = p.Id,
                                    Name = p.Name
                                })
                                .ToList();

            ViewBag.Agencies =
                new SelectList(agencies, "Id", "Name", requestModel.AgencyId);

            var processStatus
                    = ProccesStatusService
                            .AsQueryable()
                            .Select(u => new ProccesStatusModel
                            {
                                Id = u.Id,
                                Name = u.Name
                            })
                            .ToList();

            ViewBag.ProcessStatus = new SelectList(processStatus, "Id", "Name", requestModel.ProccesStatusId); ;

            ViewBag.Process = Process;
        }

    }
}