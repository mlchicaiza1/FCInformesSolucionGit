using FCInformesSolucion.Common;
using FCInformesSolucion.DAL;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FCInformesSolucion.Services
{
    public interface IRequestService : IMaintenanceService<int, Request, RequestModel>
    {
        Task<string> GetAtachmentPathAsync(int id, int index);

        Task<SaveResult> AnulmmentAsync(int id);
        Task<SaveResult> ValidateAsync(int id);
        Task<SaveResult> ProcessAsync(RequestProcessViewModel requestModel);
        Task<SaveResult> ValidateProcessAsync(int id);
    }
}