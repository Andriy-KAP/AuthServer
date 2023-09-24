using AuthServer.Core.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Core
{
    public interface IAccountService
    {
        Task<IEnumerable<IdentityError>> Register(RegisterModel model);
    }
}
