using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebProject.Areas.Identity.Data;
using WebProject.Code;
using WebProject.Data;

namespace WebProject.Areas.Admin.Pages
{
    public class UsersModel : PageModel
    {
        readonly UserContext DB;
        readonly UserManager<ApplicationUser> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        public List<UsersDataForAdmin> UsersList { get; set; }

        public UsersModel(UserContext db,
                    UserManager<ApplicationUser> userManager,
                    RoleManager<IdentityRole> roleManager)
        {
            DB = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task OnGetAsync()
        {
            await LoadUsers();
        }
        
        public async Task<IActionResult> OnPostRoleAsync(string user, string role)
        {
            var userDb = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == user);
            
            if (userDb != null)
            {
                bool hasRole = await userManager.IsInRoleAsync(userDb, role);
                if (hasRole)
                {
                    if (role != AutoMigration.AdminRoleName || (await userManager.GetUsersInRoleAsync(role)).Count > 1)
                        await userManager.RemoveFromRoleAsync(userDb, role);
                }
                else
                    await userManager.AddToRoleAsync(userDb, role);
            }
            await LoadUsers();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(string user)
        {
            var userDb = await userManager.Users.FirstOrDefaultAsync(x => x.Id == user);
            await userManager.DeleteAsync(userDb);
            await LoadUsers();
            return Page();
        }
        async Task LoadUsers()
        {
            var roles = roleManager.Roles.Select(x => x.Name);
            var usersInRole = new Dictionary<string, string[]>();
            foreach (var role in roles)
                usersInRole.Add(role, (await userManager.GetUsersInRoleAsync(role))
                    .Select(x => x.UserName).ToArray());

            UsersList = await userManager.Users.Select(x => new UsersDataForAdmin()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
            }).ToListAsync();

            foreach (var user in UsersList)
                user.Roles = usersInRole.Where(x => x.Value.Contains(user.UserName))
                    .Select(x => x.Key).ToArray();
        }

        public class UsersDataForAdmin
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string[] Roles { get; set; }
        }
    }
}
