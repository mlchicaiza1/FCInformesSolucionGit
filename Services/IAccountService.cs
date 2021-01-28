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
    public interface IAccountService: IMaintenanceService<string, ApplicationUser, UserModel>
    {
        Task AddRoles(string[] roles);    
        Task AddToRolesAsync(string id, string[] roles);
        Task AddToMenusAsync(string id, int[] menuIds);
    }
}