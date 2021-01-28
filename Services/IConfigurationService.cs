using FCInformesSolucion.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FCInformesSolucion.Services
{
    public interface IConfigurationService
    {
        Task ValidateAsync();
        Task InitAsync();
        Configuration GetConfiguration();
    }
}