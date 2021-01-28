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
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FCInformesSolucion.Controllers
{
    [Authorize(Roles = "user_access")]
    public class UserAccessController : BaseController
    {
        IAccountService AccountService;
        IMenuService MenuService;
        public UserAccessController(
            IMapper mapper,
            IAccountService accountService,
            IMenuService menuService)
        {
            AccountService = accountService;
            MenuService = menuService;

            ViewBag.Title = "Permisos";
        }

        public async Task<ActionResult> Index(string applicationUserId) {

            var users = await AccountService.AsQueryable()
                            .Where(a => a.Status == EntityStatus.Active)
                            .Select(a => new UserModel
                            {
                                Id = a.Id,
                                UserFullName = a.UserFullName
                            })
                            .ToListAsync();
            
            if (!string.IsNullOrWhiteSpace(applicationUserId))
            {
                var userMenus = MenuService.GetUserMenus(applicationUserId);
                var menuList = await MenuService.AsQueryable()
                            .ToListAsync();

                var menus = menuList
                        .Where(m => m.ParentId == null || m.ParentId == 0)
                        .OrderBy(m => m.Order)
                        .Select(m => new MenuAccessModel
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Checked = userMenus.Any(um => um.Id == m.Id),
                            MenuItems =
                                menuList
                                    .Where(m2 =>
                                        m2.ParentId == m.Id
                                        && m2.Status == EntityStatus.Active)
                                    .Select(m2 => new MenuAccessModel
                                    {
                                        Id = m2.Id,
                                        Name = m2.Name,
                                        Checked = userMenus.Any(um => um.Id == m2.Id)
                                    })
                                    .OrderBy(m2 => m.Order)
                                    .ToList()
                        })
                        .ToList();
                ViewBag.Menus = menus;
            }
            ViewBag.Users = new SelectList(users, "Id", "UserFullName", applicationUserId);            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Save(UserAccessModel requestModel) {

            await AccountService
                .AddToMenusAsync(requestModel.ApplicationUserId, requestModel.MenuIds);


            return RedirectToAction("Index", new
            {
                requestModel.ApplicationUserId
            });
        }
    }
}