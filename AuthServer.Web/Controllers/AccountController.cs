using AuthServer.Core.Core;
using AuthServer.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                
                IEnumerable<IdentityError> errors = await _accountService.Register(model, HttpContext.Request.Scheme);
                if (errors.Any())
                {
                    foreach(var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmRegistration(string userId, string code)
        {
            if (userId == null || code == null)
                return BadRequest("UserId or code are null");
            try
            {
                var result = await _accountService.ConfirmRegistration(userId, code);
                if (result.Succeeded)
                    return Ok();
                return BadRequest("Something went wrong");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _accountService.SignIn(model);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest("Username or password is invalid");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return BadRequest(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOut();
            return Ok();
        }

        [HttpGet]
        public bool IsUserSignIn()
        {
            return _accountService.IsUserSignIn(User);
        }
    }
}
