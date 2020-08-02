using KidsManagement.Data;
using KidsManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        protected KidsManagementDbContext db;
        protected UserManager<ApplicationUser> userManager;

        public ApplicationUserService(KidsManagementDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public async Task<bool> UserExists(string username)
        {
            return await Task.Run(() => db.Users.AnyAsync(u => u.NormalizedUserName == username.ToUpper()));
        }

        public async Task<IdentityResult> AssignRoles(string username, string[] roles)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username); //uses normalized name
            var result = await userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}
