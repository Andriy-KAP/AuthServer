using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}