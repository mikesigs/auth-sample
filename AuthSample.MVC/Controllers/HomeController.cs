using System.Web.Mvc;
using AuthSample.MVC.Auth;
using Thinktecture.IdentityModel.Mvc;

namespace AuthSample.MVC.Controllers
{
    [ResourceAuthorize(SampleResources.HomeActions.View, SampleResources.Home)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}