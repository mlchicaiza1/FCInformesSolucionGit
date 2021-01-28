using FCInformesSolucion.Common;
using FCInformesSolucion.DAL;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FCInformesSolucion.Services
{
    public class BankService : IBankService
    {        
        FCInformesContext Context;
        public BankService(FCInformesContext context)
        {
            Context = context;
        }

        public IQueryable<Bank> AsQueryable()
        {
            return Context.Banks.AsQueryable();
        }

        public async Task<SaveResult> DeleteAsync(int id)
        {
            var entity = Context.Banks.FirstOrDefault(u => u.Id == id);
            entity.UpdateDate = DateTime.Now;
            entity.Status = EntityStatus.Inactive;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<SaveResult> SaveAsync(BankModel model)
        {
            try
            {
                var entity = model.Id > 0
                ? Context.Banks.FirstOrDefault(e => e.Id == model.Id)
                : new Bank();

                entity.Name = model.Name;                

                if (entity.Id == 0)
                {
                    Context.Banks.Add(entity);
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