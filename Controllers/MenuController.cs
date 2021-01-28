using AutoMapper;
using FCInformesSolucion.Constants;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Helpers;
using FCInformesSolucion.Models;
using FCInformesSolucion.Services;
using MvcJqGrid;
using MvcJqGrid.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FCInformesSolucion.Controllers
{
    [Authorize(Roles = "menu_admin")]
    public class MenuController : BaseMaintenanceController<Menu, MenuModel, MenuViewModel>
    {
        IMenuService MenuService;
        public MenuController(
            IMapper mapper,
            IMenuService menuService)
            :base(mapper)
        {
            MaintenanceService = 
                MenuService = menuService;

            ViewBag.Title = "Menus";
        }

        protected override IQueryable<Menu> ApplyFilters(IQueryable<Menu> generalQuery, MvcJqGrid.Filter filter)
        {
            if (filter.rules?.Any() ?? false)
            {
                foreach (var rule in filter?.rules)
                {
                    if (rule.field == "search_query")
                    {
                        var value = rule.data.ToLower().Trim();
                        generalQuery =
                                generalQuery.Where(q => q.Name.ToLower().Contains(value)
                                            || q.Description.ToLower().Contains(value)
                                            || q.ApplicationRole.ToLower().Contains(value)
                                            || q.Url.ToLower().Contains(value)
                                );
                    }
                }
            }
            return generalQuery;
        }

        protected override string[] GetRow(Menu entity)
        {
            return new[] {
                entity.Name,
                entity.Description,
                entity.Parent?.Name,
                entity.ApplicationRole,
                HttpUtility.HtmlEncode(GetActionList(entity.Id))
            };
        }

        protected override void OnNew(MenuViewModel menuViewModel)
        {
            Init();
        }

        protected override void OnNewPost(MenuViewModel menuViewModel, MenuModel model)
        {
            Init();
        }

        protected override void OnEdit(MenuViewModel menuViewModel)
        {
            Init();
        }

        protected override void OnEditPost(MenuViewModel menuViewModel, MenuModel model)
        {
            Init();
        }

        private void Init() {
            var menus = MaintenanceService
                   .AsQueryable()
                   .Where(m => m.Status == EntityStatus.Active)
                   .Select(m => new MenuModel
                   {
                       Id = m.Id,
                       Name = m.Name
                   })
                   .ToList();

            ViewBag.Menus = menus;
        }

    }
}