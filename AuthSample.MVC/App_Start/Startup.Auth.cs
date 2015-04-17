using AuthSample.MVC.Auth;
using Owin;

namespace AuthSample.MVC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseClaimsTransformation(new SampleClaimsTransformationOptions());
            app.UseResourceAuthorization(new SampleAuthorizationPolicies());
        }
    }
}