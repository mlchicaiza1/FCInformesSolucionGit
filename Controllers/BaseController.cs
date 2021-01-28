using FCInformesSolucion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FCInformesSolucion.Controllers
{
    public class BaseController : Controller
    {

        public virtual IHtmlString GetActionList(object id)
        {
            var actions = GridHelperExts.ActionsList("")
                .Add(GetEditAction(id))
                .Add(GetDeleteAction(id))
            .End();
            return actions;
        }
        public virtual IHtmlString GetEditAction(object id)
        {
            return GridHelperExts.EditAction(Url.Action("Edit"), id);
        }

        public virtual IHtmlString GetViewAction(object id)
        {
            return GridHelperExts.ViewAction(Url.Action("View"), id);
        }


        public virtual IHtmlString GetDeleteAction(object id)
        {
            return GridHelperExts.DeleteAction(Url.Action("Delete"), "maintenance-grid", id);
        }
    }
}