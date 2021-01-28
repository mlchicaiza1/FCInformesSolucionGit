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
    [Authorize(Roles = "city_admin")]
    public class CityController : BaseMaintenanceController<City, CityModel, CityViewModel>
    {
        ICityService CityService;
        IProvinceService ProvinceService;
        public CityController(
            IMapper mapper,
            ICityService cityService,
            IProvinceService provinceService)
            :base(mapper)
        {
            MaintenanceService = 
                CityService = cityService;
            ProvinceService = provinceService;

            ViewBag.Title = "Ciudades";
        }

        protected override IQueryable<City> ApplyFilters(IQueryable<City> generalQuery, MvcJqGrid.Filter filter)
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
                                );
                    }
                }
            }
            return generalQuery;
        }

        protected override string[] GetRow(City entity)
        {
            return new[] {
                entity.Name,
                entity.Province.Name,
                HttpUtility.HtmlEncode(GetActionList(entity.Id))
            };
        }

        protected override void OnNew(CityViewModel cityViewModel)
        {
            Init();
        }

        protected override void OnNewPost(CityViewModel cityViewModel, CityModel model)
        {
            Init();
        }

        protected override void OnEdit(CityViewModel cityViewModel)
        {
            Init();
        }

        protected override void OnEditPost(CityViewModel cityViewModel, CityModel model)
        {
            Init();
        }

        private void Init() {

            var provinces = ProvinceService
                   .AsQueryable()
                   .Where(m => m.Status == EntityStatus.Active)
                   .Select(m => new ProvinceModel
                   {
                       Id = m.Id,
                       Name = m.Name
                   })
                   .ToList();

            ViewBag.Provinces = provinces;
        }

        [AllowAnonymous]
        public ActionResult GetByProvince(int provinceId) {

            var cities = CityService.AsQueryable()
                            .Where(c => c.ProvinceId == provinceId
                                    && c.Status == EntityStatus.Active)
                            .Select(c => new CityModel
                            {
                                Id = c.Id,
                                Name = c.Name
                            });

            return Json(new
            {
                success = true,
                data = cities
            }, JsonRequestBehavior.AllowGet);
        }
    }
}