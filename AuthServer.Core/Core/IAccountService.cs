using AuthServer.Core.Model;
using AuthServer.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Core
{
    public interface IAccountService
    {
        Task<IEnumerable<IdentityError>> Register(RegisterModel model, string sheme);
        Task<SignInResult> SignIn(SignInModel model);
        Task SignOut();
        Task<IdentityResult> ConfirmRegistration(string userId, string code);
        Task ChangeUserPassword(ChangeUserPasswordModel model, IPasswordValidator<ApplicationUser> validator, IPasswordHasher<ApplicationUser> hasher);
        bool IsUserSignIn(ClaimsPrincipal principal);
    }
}
