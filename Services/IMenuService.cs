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
    public interface IMenuService : IMaintenanceService<int, Menu, MenuModel>
    {
        List<MenuModel> GetUserMenus(string applicationUserId);
        List<MenuModel> GetUserMenusTree(string applicationUserId);
    }
}