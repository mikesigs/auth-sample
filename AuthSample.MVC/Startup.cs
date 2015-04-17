using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuthSample.MVC.Startup))]
namespace AuthSample.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
