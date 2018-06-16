using FindMyPet.MVC.DataLoaders;
using FindMyPet.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(FindMyPet.MVC.Startup))]
namespace FindMyPet.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        public void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            /*** Create Default Roles and Admin User on Identity Model ***/
            var rolesToCreate = ConfigurationManager.AppSettings["RolesToCreate"].ToString();
            var roles = rolesToCreate.Split(',');
            foreach (var item in roles)
            {
                var role = new IdentityRole() { Name = item };
                roleManager.Create(role);
            }

            var adminEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
            var adminPwd = ConfigurationManager.AppSettings["AdminPwd"].ToString();
            var adminRole = ConfigurationManager.AppSettings["AdminRole"].ToString();

            var adminUser = new ApplicationUser()
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var chkUser = UserManager.Create(adminUser, adminPwd);

            if (chkUser.Succeeded)
                UserManager.AddToRole(adminUser.Id, adminRole);
            /**************************/

            /***  Default Admin User in System ***/
            var ownerDataLoader = new OwnerDataLoader();
            ownerDataLoader.RegisterOwner(adminUser.Id, "Admin", "Admin", adminEmail);
            /*************************************/

        }
    }
}
