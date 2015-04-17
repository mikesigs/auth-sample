using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;

namespace AuthSample.MVC.Auth
{
    public static class WindowsClaimsTransformer
    {
        public static ICollection<Claim> GetClaimsFromWindowsPrincipal(IPrincipal principal)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, principal.Identity.Name)
            };

            var windowsIdentity = principal.Identity as WindowsIdentity;
            if (windowsIdentity == null)
            {
                throw new SecurityException(
                    String.Format("Expected WindowsIdentity but found {0}. Do you have Windows Authentication enabled?", principal.Identity.GetType().Name));
            }
            
            claims.AddRange(GetRoleClaims(windowsIdentity));
            //claims.Add(GetDisplayNameClaim(identity));
            return claims;
        }

        private static IEnumerable<Claim> GetRoleClaims(WindowsIdentity identity)
        {
            var claims = new List<Claim>();
            if (identity != null && identity.Groups != null)
            {
                foreach (var sid in identity.Groups)
                {
                    // Messy way of extracting group names
                    try
                    {
                        var translatedSid = sid.Translate(typeof(NTAccount)).Value;
                        var role = translatedSid.Substring(translatedSid.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        if (String.IsNullOrWhiteSpace(role)) { continue; }
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    catch (Exception)
                    {
                        // no-op
                    }
                }
            }

            return claims;
        }

        private static Claim GetDisplayNameClaim(WindowsIdentity identity)
        {
            var displayName = identity.Name;
            using (var pc = new PrincipalContext(ContextType.Domain, "obsglobal.com"))
            {
                var userPrincipal = UserPrincipal.FindByIdentity(pc, identity.Name);
                if (userPrincipal != null)
                {
                    displayName = userPrincipal.DisplayName;
                }
            }

            return new Claim(ClaimTypes.GivenName, displayName);
        }
    }
}