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


       
        public static string FacebookProfilePicture(this ClaimsPrincipal principal)
        {
            var claim = principal.Claim("facebookUserPicture");//principal.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return claim != null ? string.Format("http://graph.facebook.com/{0}/picture?type=large", claim) : string.Empty;
        }
        public static string FacebookProfileSmallPicture(this ClaimsPrincipal principal )
        {
            return  principal.Claim("facebookUserPicture");//  principal.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        public static string FaceBookProfilePictureFormat(string facebookUserId)
        {
            return string.Format("//graph.facebook.com/{0}/picture", facebookUserId);
        }
        
    }
}
