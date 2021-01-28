using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Web;
using System.Globalization;
using System.Threading;
using Microsoft.AspNet.Identity.EntityFramework;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.DAL;

namespace FCInformesSolucion.Controllers
{
   

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<FCInformesContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            return manager;
        }
    }
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {

            var userManager = context.GetUserManager<ApplicationUserManager>();

            return new ApplicationSignInManager(userManager, context.Authentication);
        }
    }
    public static class CustomClaimsTypes
    {
        public static string UsuarioId = "http://schemas.arpsis.com.ec/ws/2017/01/identity/claims/usuarioId";
        public static string UsuarioNombresCompletos = "http://schemas.arpsis.com.ec/ws/2017/01/identity/claims/usuarioNombresCompletos";
        public static string UsuarioEmpresaId = "http://schemas.arpsis.com.ec/ws/2017/01/identity/claims/usuarioEmpresaId";
        public static string UsuarioAgenciaId = "http://schemas.arpsis.com.ec/ws/2017/01/identity/claims/usuarioAgenciaId";
        public static string UsuarioMonedaId = "http://schemas.arpsis.com.ec/ws/2017/01/identity/claims/usuarioMonedaId";
        public static string UsuarioCulturaId = "http://schemas.arpsis.com.ec/ws/2017/01/identity/claims/usuarioCultutaId";
    }

    public static class CustomIdentityExtensions
    {
        public static string GetUserFullName(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return "";
            }
            return claimsIdentity.HasClaim(c=> c.Type == CustomClaimsTypes.UsuarioNombresCompletos)
                ? claimsIdentity.FindFirst(CustomClaimsTypes.UsuarioNombresCompletos).Value
                : "";
        }        
    }
}
