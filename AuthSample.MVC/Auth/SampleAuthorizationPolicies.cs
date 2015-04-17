using System;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace AuthSample.MVC.Auth
{
    public class SampleAuthorizationPolicies : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var resource = context.Resource.First().Value;
            switch (resource)
            {
                case SampleResources.Home:
                    return CheckHomeAccessAsync(context);
                case SampleResources.Profile:
                    return CheckProfileAccessAsync(context);
                default:
                    return Nok();
            }
        }

        private Task<bool> CheckHomeAccessAsync(ResourceAuthorizationContext context)
        {
            return context.Principal.Identity.IsAuthenticated ? Ok() : Nok();
        }

        private Task<bool> CheckProfileAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated) { return Nok(); }

            var action = context.Action.First().Value;
            switch (action)
            {
                case SampleResources.ProfileActions.List:
                    return Eval(context.Principal.IsInRole("Canpotex - Admin"));
                case SampleResources.ProfileActions.Edit:
                    return CheckProfileEditAccessAsync(context);
            }

            return Ok();
        }

        private Task<bool> CheckProfileEditAccessAsync(ResourceAuthorizationContext context)
        {
            var profileUser = context.Resource.Skip(1).Take(1).Single().Value;
            var username = context.Principal.Identity.Name;
            username = username.Substring(username.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase) + 1);
            return Eval(profileUser == username);
        }
    }
}