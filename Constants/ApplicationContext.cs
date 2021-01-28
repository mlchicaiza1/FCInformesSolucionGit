using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Constants
{
    public class ApplicationContext
    {
        public static int PageSize = 15;

        public static List<string> ApplicationRoles = new List<string> {
            "user_admin", "menu_admin", "request", "request_process", "province_admin", "city_admin", "bank_admin", "user_access", "agency_admin "
        };

        public static string DateFormat = ConfigurationManager.AppSettings["DateFormat"];
        public static string DateTimeFormat = ConfigurationManager.AppSettings["DateTimeFormat"];

        public static string CurrencyDecimalSeparator = ConfigurationManager.AppSettings["CurrencyDecimalSeparator"];
        public static string CurrencyGroupSeparator = ConfigurationManager.AppSettings["CurrencyGroupSeparator"];
        public static string CurrencySymbol = ConfigurationManager.AppSettings["CurrencySymbol"];

        public static string NumberDecimalSeparator = ConfigurationManager.AppSettings["NumberDecimalSeparator"];
        public static string NumberGroupSeparator = ConfigurationManager.AppSettings["NumberGroupSeparator"];
    }
}