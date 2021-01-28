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
    [Authorize(Roles = "user_admin")]
    public class UserController : BaseController
    {
        IAccountService AccountService;
        public UserController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(GridSettings gridSettings)
        {
            var page = gridSettings.PageIndex;

            var generalQuery = AccountService
                                .AsQueryable()
                                .Where(e=> e.Status == DAL.Entities.EntityStatus.Active);

            if (gridSettings.Where?.rules?.Any() ?? false)
            {
                foreach (var rule in gridSettings.Where?.rules)
                {
                    if (rule.field == "search_query")
                    {
                        var value = rule.data.ToLower().Trim();
                        generalQuery = 
                                generalQuery.Where(q=> q.UserFullName.ToLower().Contains(value)
                                            || q.UserName.ToLower().Contains(value)
                                            || q.Email.ToLower().Contains(value)
                                            || q.PhoneNumber.ToLower().Contains(value)
                                );
                    }
                }
            }

            var total = generalQuery.Count();
            var sort = gridSettings.SortOrder == "asc" ? SortOrder.Asc : SortOrder.Desc;
            generalQuery = generalQuery.SortAndPage(gridSettings.SortColumn, sort, page, ApplicationContext.PageSize);
            var totalPages = (int)Math.Ceiling((decimal)total / ApplicationContext.PageSize);

            var data = new
            {
                total = totalPages,
                page,
                records = total,
                rows = generalQuery.ToList().Select(item => new
                {
                    id = item.Id, // ID único de la fila
                    cell = new[] {
                        item.UserName,
                        item.UserFullName,
                        item.Email,
                        HttpUtility.HtmlEncode(GetActionList(item.Id)),
                    }
                })
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> New(NewUserViewModel requestModel)
        {
            if (ModelState.IsValid)
            {
                var model = new UserModel {
                    UserFullName = requestModel.UserFullName,
                    UserName = requestModel.UserName,
                    Email = requestModel.Email,
                    PhoneNumber = requestModel.PhoneNumber,
                    Password = requestModel.Password
                };
                var saveResult = await AccountService.SaveAsync(model);
                if (saveResult.Succeeded)
                {
                    return View("Index");
                }
                ModelState.AddModelError("", saveResult.GetErrorsString());
            }
            return View();
        }


        public ActionResult Edit(string id)
        {   
            var userModel = AccountService.AsQueryable().FirstOrDefault(u => u.Id == id);
            if (userModel != null)
            {
                var model = new EditUserViewModel
                {
                    Id = id,
                    UserFullName = userModel.UserFullName,
                    PhoneNumber = userModel.PhoneNumber,
                    Email = userModel.Email,
                    UserName = userModel.UserName
                };
                return View(model);
            }
            return new HttpNotFoundResult();

        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel requestModel)
        {
            if (ModelState.IsValid)
            {
                var model = new UserModel
                {
                    Id = requestModel.Id,
                    UserFullName = requestModel.UserFullName,
                    UserName = requestModel.UserName,
                    Email = requestModel.Email,
                    PhoneNumber = requestModel.PhoneNumber,
                    Password = requestModel.Password
                };
                var saveResult = await AccountService.SaveAsync(model);
                if (saveResult.Succeeded)
                {
                    return View("Index");
                }
                ModelState.AddModelError("", saveResult.GetErrorsString());
            }
            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {   
            var saveResult = await AccountService.DeleteAsync(id);
            if (saveResult.Succeeded)
            {
                return Json(new
                {
                    success = true,
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = false,
                message = saveResult.GetErrorsString()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}