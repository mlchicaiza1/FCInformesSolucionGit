using AutoMapper;
using FCInformesSolucion.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FCInformesSolucion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            {
                Database.SetInitializer
                (
                    new MigrateDatabaseToLatestVersion<FCInformesContext, FCInformesSolucion.Migrations.Configuration>()
                );
                using (var db = new FCInformesContext())
                {
                    
                };
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new CultureAwareControllerActivator()));
        }
    }
}
