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
    public class MenuService : IMenuService
    {        
        FCInformesContext Context;
        public MenuService(FCInformesContext context)
        {
            Context = context;
        }

        public IQueryable<Menu> AsQueryable()
        {
            return Context.Menus.AsQueryable();
        }

        public async Task<SaveResult> DeleteAsync(int id)
        {
            var entity = Context.Menus.FirstOrDefault(u => u.Id == id);
            entity.UpdateDate = DateTime.Now;
            entity.Status = EntityStatus.Inactive;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }

        public async Task<SaveResult> SaveAsync(MenuModel model)
        {
            try
            {
                var entity = model.Id > 0
                ? Context.Menus.FirstOrDefault(e => e.Id == model.Id)
                : new Menu();

                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Url = model.Url;
                entity.Order = model.Order;
                entity.ApplicationRole = model.ApplicationRole;
                entity.ParentId = model.ParentId == 0 ? default(int?) : model.ParentId;

                if (entity.Id == 0)
                {
                    Context.Menus.Add(entity);
                }

                await Context.SaveChangesAsync();

                return SaveResult.Success(entity);
            }
            catch (Exception ex)
            {
                return ex.SaveResult();
            }
        }

        public List<MenuModel> GetUserMenusTree(string applicationUserId) {

            try
            {
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));

                var roles = userManager.GetRoles(applicationUserId);

                var menus = Context.Menus
                                .Where(m => (roles.Contains(m.ApplicationRole)
                                    || m.ParentId == null || m.ParentId == 0)
                                    && m.Status == EntityStatus.Active)
                                .ToList();

                var menusResponse = new List<MenuModel>();

                menusResponse = menus
                                .Where(m => m.ParentId == null || m.ParentId == 0)
                                .OrderBy(m => m.Order)
                                .Select(m => new MenuModel
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    Description = m.Description,
                                    ApplicationRole = m.ApplicationRole,
                                    Order = m.Order,
                                    ParentId = m.ParentId ?? 0,
                                    Url = m.Url,
                                    MenuItems =
                                        menus
                                            .Where(m2 =>
                                                m2.ParentId == m.Id
                                                && m2.Status == EntityStatus.Active)
                                            .Select(m2 => new MenuModel
                                            {
                                                Id = m2.Id,
                                                Name = m2.Name,
                                                Description = m2.Description,
                                                ApplicationRole = m2.ApplicationRole,
                                                Order = m2.Order,
                                                ParentId = m2.ParentId ?? 0,
                                                Url = m2.Url,
                                            })
                                            .OrderBy(m2 => m.Order)
                                            .ToList()
                                })
                                .ToList();

                menusResponse = menusResponse
                            .Where(m => !string.IsNullOrWhiteSpace(m.Url)
                                || m.MenuItems.Any()).ToList();

                return menusResponse;
            }
            catch (Exception)
            {
                return new List<MenuModel>();
            }
        }

        public List<MenuModel> GetUserMenus(string applicationUserId)
        {
            try
            {
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));

                var roles = userManager.GetRoles(applicationUserId);

                var menus = Context.Menus
                                .Where(m => (roles.Contains(m.ApplicationRole)
                                    || m.ParentId == null || m.ParentId == 0)
                                    && m.Status == EntityStatus.Active)
                                .Select(m => new MenuModel
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    Description = m.Description,
                                    ApplicationRole = m.ApplicationRole,
                                    Order = m.Order,
                                    ParentId = m.ParentId ?? 0,
                                    Url = m.Url
                                })
                                .ToList();

                menus = menus
                        .Where(m =>
                            m.ParentId > 0
                            || (m.ParentId == 0 && menus.Any(mm => mm.ParentId == m.Id))
                        )
                        .ToList();
                return menus;
            }
            catch (Exception)
            {
                return new List<MenuModel>();
            }
        }
    }
}