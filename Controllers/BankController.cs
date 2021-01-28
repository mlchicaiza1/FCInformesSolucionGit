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
    [Authorize(Roles = "bank_admin")]
    public class BankController : BaseMaintenanceController<Bank, BankModel, BankViewModel>
    {
        IBankService BankService;
        public BankController(
            IMapper mapper,
            IBankService bankService)
            :base(mapper)
        {
            MaintenanceService = 
                BankService = bankService;

            ViewBag.Title = "Bancos";
        }

        protected override IQueryable<Bank> ApplyFilters(IQueryable<Bank> generalQuery, MvcJqGrid.Filter filter)
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

        protected override string[] GetRow(Bank entity)
        {
            return new[] {
                entity.Name,                
                HttpUtility.HtmlEncode(GetActionList(entity.Id))
            };
        }      

    }
}