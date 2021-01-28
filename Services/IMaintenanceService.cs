using FCInformesSolucion.Common;
using FCInformesSolucion.DAL;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FCInformesSolucion.Services
{
    public interface IMaintenanceService<TKey, TEntity, TModel>
    {        
        Task<SaveResult> SaveAsync(TModel model);        
        IQueryable<TEntity> AsQueryable();
        Task<SaveResult> DeleteAsync(TKey id);        
    }
}