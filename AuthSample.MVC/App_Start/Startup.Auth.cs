using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AuthSample.MVC.Auth;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace AuthSample.MVC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseClaimsTransformation(TransformClaims);
            app.UseResourceAuthorization(new SampleAuthorizationPolicies());
        }

        private static Task<ClaimsPrincipal> TransformClaims(ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                return Task.FromResult(claimsPrincipal);
            }

            var windowsIdentity = claimsPrincipal.Identity as WindowsIdentity;
            if (windowsIdentity == null)
            {
                throw new SecurityException(
                    String.Format("Expected WindowsIdentity but found {0}. Do you have Windows Authentication enabled?", claimsPrincipal.Identity.GetType().Name));
            }

            var applicationPrincipal = TransformWindowsPrincipal(windowsIdentity);
            return Task.FromResult(applicationPrincipal);
        }

        private static ClaimsPrincipal TransformWindowsPrincipal(WindowsIdentity identity)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, identity.Name)
            };

            var roleClaims = WindowsClaimsTransformer.GetRoleClaims(identity);
            claims.AddRange(roleClaims);
            claims.Add(WindowsClaimsTransformer.GetDisplayNameClaim(identity));

            var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom", ClaimTypes.Name, ClaimTypes.Role));
            return newPrincipal;
        }

    }
}