using FCInformesSolucion.Common;
using FCInformesSolucion.DAL;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FCInformesSolucion.Services
{
    public class RequestService : IRequestService
    {        
        FCInformesContext Context;
       
        public RequestService(FCInformesContext context)
        {
            Context = context;
            
        }

        public async Task<SaveResult> AnulmmentAsync(int id)
        {
            var entity = Context.Requests.FirstOrDefault(u => u.Id == id);
            entity.UpdateDate = DateTime.Now;
            entity.RequestStatus = RequestStatus.Anulmment;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<SaveResult> ValidateAsync(int id)
        {
            var entity = Context.Requests.FirstOrDefault(u => u.Id == id);
            entity.RequestStatus = RequestStatus.Validated;            

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<SaveResult> ProcessAsync(RequestProcessViewModel requestModel)
        {
            var entity = Context.Requests.FirstOrDefault(u => u.Id == requestModel.Id);

            if (entity == null)
            {
                throw new Exception($"No existe solicitud {requestModel.Id}");
            }
            
            entity.RequestProcessStatus = (RequestStatus)(requestModel.RequestProcessStatus);
            entity.ProccesStatusId = requestModel.ProccesStatusId > 0
                            ? requestModel.ProccesStatusId
                            : default(int?);
            entity.SuggestedInitialQuota = requestModel.SuggestedInitialQuota;
            entity.ProcessedDate = Convert.ToDateTime(requestModel.ProcessedDate);
            entity.ProcessRemarks = requestModel.ProcessRemarks;
            

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<SaveResult> ValidateProcessAsync(int id)
        {
            var entity = Context.Requests.FirstOrDefault(u => u.Id == id);
            entity.RequestProcessStatus = RequestStatus.Validated;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public IQueryable<Request> AsQueryable()
        {
            return Context.Requests.AsQueryable();
        }

        public async Task<SaveResult> DeleteAsync(int id)
        {
            var entity = Context.Requests.FirstOrDefault(u => u.Id == id);
            entity.UpdateDate = DateTime.Now;
            entity.Status = EntityStatus.Inactive;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<string> GetAtachmentPathAsync(int id, int index)
        {
            var configuration = await Context
                            .Configurations
                            .FirstOrDefaultAsync(c => c.Status == EntityStatus.Active);

            if (configuration == null)
            {
                throw new Exception("No existe registro de configuración en la tabla de configuraciones");
            }

            var entity = Context.Requests.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                throw new Exception($"No existe solicitud con id={id}");
            }

            if (index == 1)
            {
                return Path.Combine(configuration.FilesPath, entity.Attachment1Name);
            }
            if (index == 2)
            {
                return Path.Combine(configuration.FilesPath, entity.Attachment2Name);
            }
            if (index == 3)
            {
                return Path.Combine(configuration.FilesPath, entity.Attachment3Name);
            }
            if (index == 4)
            {
                return Path.Combine(configuration.FilesPath, entity.Attachment4Name);
            }
            return "";           

        }
        public async Task<SaveResult> SaveAsync(RequestModel model)
        {
            try
            {
                var configuration = await Context
                            .Configurations
                            .FirstOrDefaultAsync(c => c.Status == EntityStatus.Active);

                if (configuration == null)
                {
                    throw new Exception("No existe registro de configuración en la tabla de configuraciones");
                }

                var entity = model.Id > 0
                ? Context.Requests.FirstOrDefault(e => e.Id == model.Id)
                : new Request();

                entity.AgencyId = model.AgencyId;
                entity.RequestNumber = model.RequestNumber;
                entity.RequestDate = model.RequestDate;
                entity.UserName = model.UserName;
                entity.Identification = model.Identification;
                entity.FullNames = model.FullNames;
                entity.ProvinceId = model.ProvinceId;
                entity.CityId = model.CityId;
                entity.Place = model.Place;
                entity.Telephone = model.Telephone;
                entity.Cellphone = model.Cellphone;
                entity.Address = model.Address;
                entity.BankId = model.BankId;
                entity.ApplicantName = model.ApplicantName;
                entity.Email = model.Email;
                entity.RequestStatus = (RequestStatus)(model.RequestStatus);

                if (model.Attachment1 != null && model.Attachment1.Length > 0)
                {
                    if (!Directory.Exists(configuration.FilesPath))
                    {
                        Directory.CreateDirectory(configuration.FilesPath);
                    }
                    var attachment1Path = Path.Combine(configuration.FilesPath, model.Attachment1Name);
                    File.WriteAllBytes(attachment1Path, model.Attachment1);
                    entity.Attachment1Name = model.Attachment1Name;
                }

                if (model.Attachment2 != null && model.Attachment2.Length > 0)
                {
                    if (!Directory.Exists(configuration.FilesPath))
                    {
                        Directory.CreateDirectory(configuration.FilesPath);
                    }
                    var attachment2Path = Path.Combine(configuration.FilesPath, model.Attachment2Name);
                    File.WriteAllBytes(attachment2Path, model.Attachment2);
                    entity.Attachment2Name = model.Attachment2Name;
                }

                if (model.Attachment3 != null && model.Attachment3.Length > 0)
                {
                    if (!Directory.Exists(configuration.FilesPath))
                    {
                        Directory.CreateDirectory(configuration.FilesPath);
                    }
                    var attachment3Path = Path.Combine(configuration.FilesPath, model.Attachment3Name);
                    File.WriteAllBytes(attachment3Path, model.Attachment3);
                    entity.Attachment3Name = model.Attachment3Name;
                }

                if (model.Attachment4 != null && model.Attachment4.Length > 0)
                {
                    if (!Directory.Exists(configuration.FilesPath))
                    {
                        Directory.CreateDirectory(configuration.FilesPath);
                    }
                    var attachment4Path = Path.Combine(configuration.FilesPath, model.Attachment4Name);
                    File.WriteAllBytes(attachment4Path, model.Attachment4);
                    entity.Attachment4Name = model.Attachment4Name;
                }

                if (entity.Id == 0)
                {
                    entity.RequestNumber = configuration.RequestNextNumber++;
                    Context.Requests.Add(entity);
                }

                await Context.SaveChangesAsync();

                return SaveResult.Success(entity);
            }
            catch (Exception ex)
            {
                return ex.SaveResult();
            }
        }
    }
}