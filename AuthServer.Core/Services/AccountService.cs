using AuthServer.Core.Converters;
using AuthServer.Core.Core;
using AuthServer.Core.Model;
using AuthServer.Domain.Model;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<IEnumerable<IdentityError>> Register(RegisterModel model, string sheme)
        {
            ApplicationUser user = ApplicationUserConverter.Convert(model);
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = $"https://localhost:7192/api/Account/Confirm?userId={user.Id}&code={code}";
                await _emailService.SendEmail(user.Email, "Confirm your account", $"<a href='{callbackUrl}'>Confirmation link</a>");
                return Enumerable.Empty<IdentityError>();
            }
            else
            {
                return result.Errors;
            }
        }

        public async Task<SignInResult> SignIn(SignInModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public bool IsUserSignIn(ClaimsPrincipal principal)
        {
            return _signInManager.IsSignedIn(principal);
        }
    }
}
