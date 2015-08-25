using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access.Extensions
{
    public static class TokenExtensions
    {
        public static string Claim(this ClaimsPrincipal principal, string key)
        {
            var claim = principal.FindFirst(key);
            return claim != null ? claim.Value : null;
        }

        public static string Claim(this ClaimsIdentity identity, string key)
        {
            var claim = identity.FindFirst(key);

            return claim != null ? claim.Value : null;
        }

        public static int? CenterId (this ClaimsPrincipal principal)
        {
            var claim = principal.Claim("CenterId");

            return !string.IsNullOrEmpty(claim) ?( int?) Convert.ToInt32(claim) : null;
        }

        
    }
}
