using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services
{
    public interface IApplicationUserService
    {
       Task<bool> UserExists(string username);

        Task<IdentityResult> AssignRoles(string username, string[] roles);
    }
}
