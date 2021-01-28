using FCInformesSolucion.Constants;
using FCInformesSolucion.DAL;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FCInformesSolucion.Services
{
    public class ConfigurationService: IConfigurationService
    {
        FCInformesContext Context;
        IAccountService UserService;
        public ConfigurationService(FCInformesContext context,
                IAccountService userService)
        {
            Context = context;
            UserService = userService;
        }


        public async Task ValidateAsync() {
            await InitAsync();
        }

        public async Task InitAsync() {
            try
            {
                var roles = ApplicationContext.ApplicationRoles.ToArray();

                var existRoles = Context.Roles.Any();
                if (!existRoles)
                {
                    await UserService.AddRoles(roles);
                }

                var existUser = Context.Users.Any();
                if (!existUser)
                {
                    var userModel = new UserModel
                    {
                        UserName = "admin",
                        Password = "Menatics@123.",
                        Email = "admin@gmail.com",
                        UserFullName = "Administrator",
                        PhoneNumber = ""
                    };
                    var result = await UserService.SaveAsync(userModel);
                    if (result.Succeeded)
                    {   
                        var applicationUser = (ApplicationUser)result.Entity;
                        await UserService.AddToRolesAsync(applicationUser.Id,
                                roles
                            );
                    }
                }

                var existsMenus = Context.Menus.Any();
                if (!existsMenus)
                {
                    var processMenu = new Menu
                    {
                        Name = "Procesos",
                        Description = "Procesos",
                        Order = 1,
                        Status = EntityStatus.Active,                        
                    };
                    Context.Menus.Add(processMenu);

                    var newRequestMenu = new Menu
                    {
                        Name = "Nueva Solicitud",
                        Description = "Nueva Solicitud",
                        Url = "~/Request",
                        ApplicationRole = "request",
                        Order = 1,
                        Status = EntityStatus.Active,
                        Parent = processMenu
                    };
                    Context.Menus.Add(newRequestMenu);

                    var processRequestMenu = new Menu
                    {
                        Name = "Procesar Solicitud",
                        Description = "Procesar Solicitud",
                        Url = "~/RequestProcess",
                        ApplicationRole = "request_process",
                        Order = 2,
                        Status = EntityStatus.Active,
                        Parent = processMenu
                    };
                    Context.Menus.Add(processRequestMenu);


                    var adminMenu = new Menu
                    {
                        Name = "Configuración Sistema",
                        Description = "Configuración Sistema",
                        Order = 2,
                        Status = EntityStatus.Active,
                    };
                    Context.Menus.Add(adminMenu);

                    var usersMenu = new Menu
                    {
                        Name = "Usuarios",
                        Description = "Usuarios",
                        Url = "~/User",
                        ApplicationRole = "user_admin",
                        Order = 1,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(usersMenu);

                    var provinceMenu = new Menu
                    {
                        Name = "Provincias",
                        Description = "Provincias",
                        Url = "~/Province",
                        ApplicationRole = "province_admin",
                        Order = 2,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(provinceMenu);

                    var cityMenu = new Menu
                    {
                        Name = "Ciudades",
                        Description = "Ciudades",
                        Url = "~/City",
                        ApplicationRole = "province_admin",
                        Order = 3,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(cityMenu);

                    var bankMenu = new Menu
                    {
                        Name = "Bancos",
                        Description = "Bancos",
                        Url = "~/Bank",
                        ApplicationRole = "bank_admin",
                        Order = 4,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(bankMenu);

                    var userAccessMenu = new Menu
                    {
                        Name = "Permisos",
                        Description = "Permisos",
                        Url = "~/UserAccess",
                        ApplicationRole = "user_access",
                        Order = 5,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(userAccessMenu);

                    var menuMenu = new Menu
                    {
                        Name = "Menus",
                        Description = "Menus",
                        Url = "~/Menu",
                        ApplicationRole = "menu_admin",
                        Order = 6,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(menuMenu);

                    var menuAgency = new Menu
                    {
                        Name = "Agencia",
                        Description = "Agencias",
                        Url = "~/Agency",
                        ApplicationRole = "agency_admin",
                        Order = 7,
                        Status = EntityStatus.Active,
                        Parent = adminMenu
                    };
                    Context.Menus.Add(menuMenu);

                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Configuration GetConfiguration()
        {
            return Context.Configurations.FirstOrDefault(c=> c.Status == EntityStatus.Active);
        }
    }
}