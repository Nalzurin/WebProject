using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProject.Areas.Identity.Data;
using WebProject.Data;

namespace WebProject.Code
{
    public class AutoMigration
    {
        public const string AdminRoleName = "admin";
        public const string UserRoleName = "user";
        readonly UserContext DB;
        readonly UserManager<ApplicationUser> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        public AutoMigration(UserContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            DB = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void Initialize()
        {
            DB.Database.MigrateAsync().Wait();
            InitUsers().Wait();
        }
        async Task InitUsers()
        {
            var adminRole = await FindOrAddRole(AdminRoleName);
            await FindOrAddRole(UserRoleName);

            var adminUser = await userManager.FindByNameAsync("admin@admin.com");
            if(adminUser == null)
            {
                adminUser = new ApplicationUser()
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true

                };
                await userManager.CreateAsync(adminUser, "admin");
            }
            bool isAdmin = await userManager.IsInRoleAsync(adminUser, AdminRoleName);
            if(!isAdmin)
            {
                await userManager.AddToRoleAsync(adminUser,AdminRoleName);
            }
        }
        async Task<IdentityRole> FindOrAddRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if(role == null)
            {
                role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);

            }
            return role;
        }
    }
}
