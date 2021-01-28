using AutoMapper;
using FCInformesSolucion.Constants;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Helpers;
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
    public abstract class BaseMaintenanceController<TEntity, TModel, TViewModel> : BaseController
        where TEntity : Entity, new()
        where TModel : class, new()
        where TViewModel : class, new()
    {
        protected IMaintenanceService<int, TEntity, TModel> MaintenanceService;

        protected readonly IMapper Mapper;
        protected abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> generalQuery, MvcJqGrid.Filter filter);
        protected abstract string[] GetRow(TEntity entity);

        protected virtual void OnNew(TViewModel viewModel) { }
        protected virtual void OnNewPost(TViewModel viewModel, TModel model) { }

        protected virtual void OnEdit(TViewModel viewModel) { }
        protected virtual void OnEditPost(TViewModel viewModel, TModel model) { }
        protected virtual void OnDelete() { }

        protected virtual void OnIndex() { }

        protected virtual ActionResult ViewRedirectOnNew(TEntity entity) {
            return View("Index");
        }
        protected virtual ActionResult ViewRedirectOnEdit(TViewModel viewModel, TModel model) {
            return View("Index");
        }

        public BaseMaintenanceController(IMapper mapper)
        {
            Mapper = mapper;
        }

        public ActionResult Index()
        {
            OnIndex();
            return View();
        }

        public ActionResult GetList(GridSettings gridSettings)
        {
            var page = gridSettings.PageIndex;

            var generalQuery = MaintenanceService
                                .AsQueryable()
                                .Where(e => e.Status == EntityStatus.Active);

            if (gridSettings.Where != null)
            {
                generalQuery = ApplyFilters(generalQuery, gridSettings.Where);            
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
                    cell = GetRow(item)
                })
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult New()
        {
            var viewModel = new TViewModel();
            OnNew(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> New(TViewModel viewModel)
        {
            var model = Mapper.Map<TModel>(viewModel);
            OnNewPost(viewModel, model);

            if (ModelState.IsValid)
            {                
                var saveResult = await MaintenanceService.SaveAsync(model);
                if (saveResult.Succeeded)
                {
                    return ViewRedirectOnNew(saveResult.Entity as TEntity);
                }
                ModelState.AddModelError("", saveResult.GetErrorsString());
            }
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {   
            var entity = MaintenanceService.AsQueryable().FirstOrDefault(u => u.Id == id);
            if (entity != null)
            {
                var viewModel = Mapper.Map<TViewModel>(entity);
                OnEdit(viewModel);
                return View(viewModel);
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TViewModel viewModel)
        {   
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<TModel>(viewModel);
                OnEditPost(viewModel, model);
                var saveResult = await MaintenanceService.SaveAsync(model);
                if (saveResult.Succeeded)
                {
                    return ViewRedirectOnEdit(viewModel, model);
                }
                ModelState.AddModelError("", saveResult.GetErrorsString());
            }
            else
            {
                OnEditPost(viewModel, null);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Delete(int id)
        {
            OnDelete();
            var saveResult = await MaintenanceService.DeleteAsync(id);
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