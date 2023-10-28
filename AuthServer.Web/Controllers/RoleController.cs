using AuthServer.Core.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(_roleService.GetRoles());
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string role)
        {
            try
            {
                IdentityResult result = await _roleService.AddRole(role);
                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest("Something went wrong.");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string id)
        {
            try
            {
                IdentityResult result = await _roleService.RemoveRole(id);
                if(result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest("Something went wrong.");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRoles(string userId, IEnumerable<string> rolesId)
        {
            try
            {
                await _roleService.AddUserToRoles(userId, rolesId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
