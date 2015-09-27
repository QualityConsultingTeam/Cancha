using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity
{
    public static class UserClaimsExtensions
    {

        public static string Find(this IEnumerable<IdentityUserClaim> claims, string key)
        {
            if (claims.All(c => c == null)) return string.Empty;
            return claims.Where(c => c.ClaimType == key)
                .Select(c => c.ClaimValue).FirstOrDefault();
        }

        public static bool HasName(this IEnumerable<IdentityUserClaim> claims)
        {
            if (claims.All(c => c == null)) return false;
            return claims.Any(c => c.ClaimType == "name"
                || c.ClaimType.Contains("first_name")
                || c.ClaimType.Contains("last_name")
                || c.ClaimType.Contains("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"));
        }
        public static string FinUserName(this IEnumerable<IdentityUserClaim> claims)
        {
            if (claims.All(c => c == null)) return string.Empty;
            var name = claims.Find("name") ?? claims.Find("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

            if (!string.IsNullOrEmpty(name)) return name;

            return string.Format("{0} {1}", claims.Find("first_name"), claims.Find("last_name"));
        }
    }
}
