using AuthServer.Core.Model;
using AuthServer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Converters
{
    internal class ApplicationUserConverter
    {
        public static ApplicationUser Convert(RegisterModel model)
        {
            return new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                City = model.City,
                Country = model.Country
            };
        }
    }
}
