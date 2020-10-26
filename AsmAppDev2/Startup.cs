using AsmAppDev2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AsmAppDev2.Startup))]
namespace AsmAppDev2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        // In this method we will create default User roles and Admin user for login    
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            // Tạo role cho database
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            // Tạo kho lưu trữ user
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role    
                var role = new IdentityRole();
                role.Name = "Admin";
                role.Id = "1";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "QuocHoan";
                user.Email = "admin@admin.com";

                string userPWD = "Quochoan1611@";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
