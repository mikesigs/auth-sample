using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace AuthSample.MVC
{
    public class ClaimsCache
    {
        public static ICollection<Claim> GetClaims(string username)
        {
            var cacheKey = CreateCacheKey(username);
            return HttpContext.Current.Cache[cacheKey] as ICollection<Claim>;
        }

        public static void SetClaims(string username, ICollection<Claim> claims)
        {
            var cacheKey = CreateCacheKey(username);
            HttpContext.Current.Cache[cacheKey] = claims;
        }

        private static string CreateCacheKey(string username)
        {
            return string.Format("AuthSampleClaims_{0}", username);
        }
    }
}