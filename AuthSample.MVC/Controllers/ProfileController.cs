using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuthSample.MVC.Auth;
using AuthSample.MVC.Models;
using Thinktecture.IdentityModel.Mvc;

namespace AuthSample.MVC.Controllers
{
    [ResourceAuthorize(SampleResources.ProfileActions.View, SampleResources.Profile)]
    public class ProfileController : Controller
    {
        private static readonly List<Profile> _profiles = new List<Profile>()
        {
            new Profile(@"obs\msigsworth", "Mike", "Sigsworth", 5),
            new Profile(@"obs\qwilson", "Quinn", "Wilson", 5),
            new Profile(@"obs\apetrov", "Arik", "Petrov", 4),
            new Profile(@"obs\dlussier", "D'Arcy", "Lussier", 3),
            new Profile(@"obs\churd", "Chad", "Hurd", 2)
        };

        // GET: Profile
        public ActionResult Edit(string profileUser)
        {
            if (!HttpContext.CheckAccess(
                SampleResources.ProfileActions.Edit,
                SampleResources.Profile,
                profileUser))
            {
                return new AccessDeniedResult();
            }

            var profile = _profiles.FirstOrDefault(p => p.UserName == profileUser);
            
            if (profile == null) { return HttpNotFound(); }

            return View(profile);
        }

        [HttpPost]
        public ActionResult Edit(Profile profile)
        {
            if (!HttpContext.CheckAccess(
                SampleResources.ProfileActions.Edit,
                SampleResources.Profile,
                profile.UserName))
            {
                return new AccessDeniedResult();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [ResourceAuthorize(SampleResources.ProfileActions.List, SampleResources.Profile)]
        public ActionResult List()
        {
            return View(_profiles);
        }
    }
}