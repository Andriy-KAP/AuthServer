using AuthServer.Core.Converters;
using AuthServer.Core.Core;
using AuthServer.Core.Model;
using AuthServer.Domain.Model;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityError>> Register(RegisterModel model)
        {
            ApplicationUser user = ApplicationUserConverter.Convert(model);
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                return Enumerable.Empty<IdentityError>();
            }
            else
            {
                return result.Errors;
            }
        }
    }
}
