using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FCInformesSolucion.DAL;
using FCInformesSolucion.Models;
using FCInformesSolucion.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace FCInformesSolucion.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IConfigurationService ConfigurationService { get; set; }

        private ApplicationUserManager _userManager;
        public AccountController(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }
        

        //public AccountController(
        //    ApplicationUserManager userManager, 
        //    ApplicationSignInManager signInManager            
        //)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
            
        //}

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ShowEmpresas = false;
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginModel { };
            return View(model);
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            await ConfigurationService.ValidateAsync();

            var appUser = await SignInManager.UserManager.FindAsync(model.UserName, model.Password);
            if (appUser != null)
            {
                var userIdentity = await UserManager
                                .CreateIdentityAsync(appUser, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, userIdentity);
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Usuario o Contraseña inválidos.");
            return View(model);
        }

        

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["ArpsisCurrentMoneda"] = null;
            Session["ArpsisCurrentCultura"] = null;
            Session["ArpsisCurrentUsuario"] = null;
            Session["ArpsisCurrentEmpresa"] = null;
            Session["ArpsisCurrentAgencia"] = null;

            AuthenticationManager.SignOut();            

            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

   
        #endregion
    }
}