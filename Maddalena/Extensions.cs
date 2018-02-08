﻿using Maddalena.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Maddalena
{
    public static class Extensions
    {
        public static async Task<ApplicationUser> ToUser(this ClaimsPrincipal claim)
        {
            if (claim?.Identity?.Name == null) return null;

            return await ApplicationUser.FirstOrDefaultAsync(x => x.UserName == claim.Identity.Name);
        }
    }
}