using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AuthSample.MVC.Auth;
using Thinktecture.IdentityModel.Owin;

namespace AuthSample.MVC
{
    public class SampleClaimsTransformationOptions : ClaimsTransformationOptions
    {
        public SampleClaimsTransformationOptions()
        {
            ClaimsTransformation = ClaimsTransformer;
        }

        private static Task<ClaimsPrincipal> ClaimsTransformer(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                return Task.FromResult(claimsPrincipal);
            }

            var username = claimsPrincipal.Identity.Name;
            if (String.IsNullOrWhiteSpace(username))
            {
                return Task.FromResult(claimsPrincipal);
            }

            return Task.FromResult(
                CreateApplicationPrincipal(claimsPrincipal));
        }

        private static ClaimsPrincipal CreateApplicationPrincipal(IPrincipal principal)
        {
            var claims = ClaimsCache.GetClaims(principal.Identity.Name);
            if (claims == null)
            {
                claims = WindowsClaimsTransformer.GetClaimsFromWindowsPrincipal(principal);
                ClaimsCache.SetClaims(principal.Identity.Name, claims);
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom", ClaimTypes.Name, ClaimTypes.Role));
        }

    }
}