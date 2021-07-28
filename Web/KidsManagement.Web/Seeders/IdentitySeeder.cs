using KidsManagement.Data;
using KidsManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection; // wow wtf must remember -> activates UserManager

using System.Threading.Tasks;
using System.Linq;
using KidsManagement.Data.Models.Enums;

namespace KidsManagement.Web.Seeders
{
    public class IdentitySeeder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly KidsManagementDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly string adminUserName = "John";
        private readonly string adminRole = "Admin";
        private readonly string adminPassword = "1234";
        private readonly string teacherUserName = "Mark";
        private readonly string teacherRole = "Teacher";
        private readonly string teacherPassword = "1234";


        public IdentitySeeder(IServiceProvider serviceProvider, KidsManagementDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.db = dbContext;
            this.userManager = this.serviceProvider.GetService<UserManager<ApplicationUser>>();
            this.roleManager = this.serviceProvider.GetService<RoleManager<ApplicationRole>>();
        }
        public async Task SeedAll()
        {
            await SeedRolesAsync(adminRole);
            await SeedRolesAsync(teacherRole);

            await SeedUsersAsync(adminUserName,adminPassword,adminRole);
            await SeedUsersAsync(teacherUserName,teacherPassword, teacherRole);

            await SeedUsersToRolesAsync(adminUserName, adminRole);
            await SeedUsersToRolesAsync(teacherUserName, teacherRole);
        }

        private async Task SeedRolesAsync(string roleName)
        {
            if (await this.roleManager.FindByNameAsync(roleName) != null)
            {
                return;
            }

            await roleManager.CreateAsync(new ApplicationRole(roleName));
        }

        private async Task SeedUsersAsync(string username, string password, string roleName)
        {
            if (await this.userManager.FindByNameAsync(username) != null)
            {
                return;
            }
            var user = new ApplicationUser
            {
                UserName = username,
                Email = $"{username}@abv.bg",
                EmailConfirmed = true,

            };
            var result=await this.userManager.CreateAsync(user,password);

            switch (roleName)
            {
                case "Admin":
                    var admin = new Admin
                    {
                        FirstName = "Svilen",
                        LastName = "Pavlov",
                        Gender = Gender.Male,
                        HireDate = DateTime.Now,
                        Salary = 1000m,
                        ApplicationUserId=user.Id
                    };
                    db.Admins.Add(admin);
                    break;

                case "Teacher":
                    var teacher = new Teacher
                    {
                        FirstName = "Mark",
                        LastName = "Tomas",
                        Gender = Gender.Female,
                        HiringDate = DateTime.Now,
                        PhoneNumber = "0888 888 888",
                        Salary = 1000m,
                        ApplicationUserId = user.Id
                    };
                    db.Teachers.Add(teacher);
                    break;
                default:
                    break;
            }

        }

        private async Task SeedUsersToRolesAsync(string username, string roleName)
        {
            var user  = await this.userManager.FindByNameAsync(username);
            var role = await this.roleManager.FindByNameAsync(roleName);
            if (user.Id == null || role.Id == null)
            {
                return;
            }

            var linkExists = db.UserRoles.Any(x => x.UserId == user.Id && x.RoleId == role.Id);

            if (linkExists)
            {
                return;
            }


            db.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = user.Id,

            });

           
            await db.SaveChangesAsync();
        }


    }
}
