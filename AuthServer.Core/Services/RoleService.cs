using AuthServer.Core.Core;
using AuthServer.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    internal class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManager.Roles;
        }
        public async Task<IdentityResult> AddRole(string role)
        {
            if(string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));
            return await _roleManager.CreateAsync(new IdentityRole(role));
        }

        public async Task AddUserToRoles(string userId, IEnumerable<string> rolesId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new NullReferenceException($"Cannot find user with id {userId}");
            }
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<string> addedRoles = rolesId.Except(userRoles).ToList<string>();
            List<string> removedRoles = userRoles.Except(rolesId).ToList<string>();

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
        }

        public async Task<IdentityResult> RemoveRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                throw new NullReferenceException("Cannot find role.");
            }
            return await _roleManager.DeleteAsync(role);
        }
    }
}
