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
    public class AccountService: IAccountService
    {
        UserManager<ApplicationUser> UserManager;
        RoleStore<ApplicationRole> RoleStore;
        RoleManager<ApplicationRole> RoleManager;
        UserStore<ApplicationUser> UserStore;
        FCInformesContext Context;
        IMenuService MenuService;
        public AccountService(FCInformesContext context,
                        IMenuService menuService)
        {
            Context = context;
            MenuService = menuService;
        }

        public async Task AddRoles(string[] roles) {
            RoleStore = new RoleStore<ApplicationRole>(Context);
            foreach (var role in roles)
            {
                var r = await RoleStore.FindByNameAsync(role);
                if (r == null )
                {
                    await RoleStore.CreateAsync(new ApplicationRole
                    {
                        Name = role
                    });
                }
            }
        }

        public async Task<SaveResult> SaveAsync(UserModel userModel)
        {
            try
            {
                var saveResult = SaveResult.Failed("No procesado");
                try
                {
                    UserStore = new UserStore<ApplicationUser>(Context);
                    UserManager = new UserManager<ApplicationUser>(UserStore);
                    RoleStore = new RoleStore<ApplicationRole>(Context);
                    RoleManager = new RoleManager<ApplicationRole>(RoleStore);                    

                    var result = IdentityResult.Failed("No procesado");

                    if (string.IsNullOrWhiteSpace(userModel.Id))
                    {   
                        var userAplication = new ApplicationUser {
                            UserFullName = userModel.UserFullName,
                            Email = userModel.Email,
                            PhoneNumber = userModel.PhoneNumber,
                            UserName = userModel.UserName,
                            CreatedDate = DateTime.Now,
                            Status = EntityStatus.Active
                        };
                        result = await UserManager.CreateAsync(userAplication, userModel.Password);
                        if (!result.Succeeded)
                        {
                            throw new Exception("Error al agregar usuario " + string.Join(",", result.Errors.ToArray()));
                        }
                        return SaveResult.Success(userAplication);
                    }
                    else
                    {
                        var userAplication = await UserManager.FindByIdAsync(userModel.Id);
                        if (userAplication != null)
                        {
                            userAplication.UserFullName = userModel.UserFullName;
                            userAplication.Email = userModel.Email;
                            userAplication.PhoneNumber = userModel.PhoneNumber;
                            userAplication.UpdateDate = DateTime.Now;
                            result = await UserManager.UpdateAsync(userAplication);
                            if (!result.Succeeded)
                            {
                                throw new Exception("Error al agregar usuario " + string.Join(",", result.Errors.ToArray()));
                            }

                            if (!string.IsNullOrWhiteSpace(userModel.Password))
                            {
                                var hashedNewPassword = UserManager.PasswordHasher.HashPassword(userModel.Password);
                                await UserStore.SetPasswordHashAsync(userAplication, hashedNewPassword);
                                await UserStore.UpdateAsync(userAplication);
                            }

                            return SaveResult.Success(userAplication);
                        }
                    }
                    return saveResult;
                }
                catch (Exception ex)
                {
                    return ex.SaveResult();
                }
            }
            catch (Exception ex)
            {
                return ex.SaveResult();
            }            
        }

        public async Task AddToRolesAsync(string id, string[] roles)
        {
            if (UserManager == null)
            {
                UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context));
            }

            var userRoles = await UserManager.GetRolesAsync(id);
            foreach (var role in roles)
            {
                if (!userRoles.Contains(role))
                {
                    await UserManager.AddToRoleAsync(id, role);
                }
            }

            foreach (var userRole in userRoles)
            {
                if (!roles.Contains(userRole))
                {
                    await UserManager.RemoveFromRoleAsync(id, userRole);
                }
            }
        }

        public async Task AddToMenusAsync(string id, int[] menuIds)
        {   

            var roles = Context.Menus
                        .Where(m => menuIds.Contains(m.Id) 
                            && m.ApplicationRole != null
                            && m.Status == EntityStatus.Active)
                        .Select(m=> m.ApplicationRole)
                        .ToArray();

            await AddToRolesAsync(id, roles);
        }


        public IQueryable<ApplicationUser> AsQueryable()
        {
            return Context.Users.AsQueryable();
        }

        public async Task<SaveResult> DeleteAsync(string id)
        {
            var entity = Context.Users.FirstOrDefault(u => u.Id == id);
            entity.Status = EntityStatus.Inactive;

            await Context.SaveChangesAsync();

            return SaveResult.Success();
        }
    }
}