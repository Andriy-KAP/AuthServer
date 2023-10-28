using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Core
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRole(string role);
        Task<IdentityResult> RemoveRole(string id);
        Task AddUserToRoles(string userId, IEnumerable<string> rolesId);
        IEnumerable<IdentityRole> GetRoles();
    }
}
