using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FCInformesSolucion.Constants;

namespace FCInformesSolucion
{
    public class CultureAwareControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            var cultura = new CultureInfo("es-EC");
            cultura.NumberFormat = new NumberFormatInfo {
                CurrencyDecimalSeparator = ApplicationContext.CurrencyDecimalSeparator,
                CurrencyGroupSeparator = ApplicationContext.CurrencyGroupSeparator,
                CurrencySymbol = ApplicationContext.CurrencySymbol,
                NumberDecimalSeparator = ApplicationContext.NumberDecimalSeparator,
                NumberGroupSeparator = ApplicationContext.NumberGroupSeparator
            };            

            cultura.DateTimeFormat.DateSeparator = "/";
            cultura.DateTimeFormat.ShortDatePattern = ApplicationContext.DateFormat;
            cultura.DateTimeFormat.LongDatePattern = ApplicationContext.DateTimeFormat;

            

            Thread.CurrentThread.CurrentCulture = cultura;
            Thread.CurrentThread.CurrentUICulture = cultura;
            
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}