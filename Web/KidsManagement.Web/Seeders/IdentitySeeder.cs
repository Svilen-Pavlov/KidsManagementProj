using KidsManagement.Data;
using KidsManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection; // wow wtf must remember -> activates UserManager

using System.Threading.Tasks;
using System.Linq;

namespace KidsManagement.Web.Seeders
{
    public class IdentitySeeder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly KidsManagementDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly string username1 = "John1";
        private readonly string role1 = "Admin1";
        private readonly string password1 = "1234";

        public IdentitySeeder(IServiceProvider serviceProvider, KidsManagementDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.db = dbContext;
            this.userManager = this.serviceProvider.GetService<UserManager<ApplicationUser>>();
            this.roleManager = this.serviceProvider.GetService<RoleManager<ApplicationRole>>();
        }
        public async Task SeedAll()
        {
            await SeedUsersAsync();
            await SeedRolesAsync();
            await SeedUsersToRolesAsync();
        }

        private async Task SeedUsersToRolesAsync()
        {
            var user = await userManager.FindByNameAsync(username1);
            var role = await roleManager.FindByNameAsync(role1);
            if (user.Id==null || role.Id==null)
            {
                return;
            }
            var exists = db.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == role.Id);

            if (exists)
            {
                return;
            }
                

            db.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId= user.Id,
                
            });

            await db.SaveChangesAsync();
        }

        private async Task SeedRolesAsync()
        {
            var role = await roleManager.FindByNameAsync(role1);
            if (role != null)
            {
                //return;
            }

            await roleManager.CreateAsync(new ApplicationRole(role1));

        }

        private async Task SeedUsersAsync()
        {
            var exists = userManager.FindByNameAsync(username1);

            if (exists != null)
            {
                //return;
            }

            var user = await this.userManager.CreateAsync(new ApplicationUser
            {
                UserName = username1,
                Email = $"{username1}@abv.bg",
                EmailConfirmed = true,
                
            },
            password1);

        }
    }
}
