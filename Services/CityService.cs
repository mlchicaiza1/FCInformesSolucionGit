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
    public class CityService : ICityService
    {        
        FCInformesContext Context;
        public CityService(FCInformesContext context)
        {
            Context = context;
        }

        public IQueryable<City> AsQueryable()
        {
            return Context.Cities.AsQueryable();
        }

        public async Task<SaveResult> DeleteAsync(int id)
        {
            var entity = Context.Cities.FirstOrDefault(u => u.Id == id);
            entity.UpdateDate = DateTime.Now;
            entity.Status = EntityStatus.Inactive;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<SaveResult> SaveAsync(CityModel model)
        {
            try
            {
                var entity = model.Id > 0
                ? Context.Cities.FirstOrDefault(e => e.Id == model.Id)
                : new City();

                entity.Name = model.Name;
                entity.ProvinceId = model.ProvinceId;

                if (entity.Id == 0)
                {
                    Context.Cities.Add(entity);
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