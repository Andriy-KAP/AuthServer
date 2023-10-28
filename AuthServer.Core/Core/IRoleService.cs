using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Core
{
    internal interface IRoleService
    {
        Task<IdentityResult> AddRole(string role);
        Task<IdentityResult> RemoveRole(string id);
        Task AddUserToRole(string userId, IEnumerable<string> rolesId);
    }
}
